using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/BulletBuffDamage")]
public class BulletBuffDamage : PowerUpEffect
{
    public float amount;

    public ManagerData managerData;

    public override void Apply(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        managerData = FindObjectOfType<ManagerData>();
        if (managerData != null)
        {
            managerData.AddDamageBullet(amount - managerData.CurrentDamageBonus); // Ajustar el daño según el nuevo bono
            Debug.Log("Damage Bonus " + managerData.CurrentDamageBonus + "Amount "+ amount);
        } 
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.damage -= managerData.CurrentDamageBonus; // Revertir cualquier bono anterior
                    bullet.damage += amount; // Aplicar el nuevo bono
                }
            }
        }
        managerData.CurrentDamageBonus = amount; // Guardar el bono actual
        Debug.Log("CurrentDamageBonuts " + managerData.CurrentDamageBonus);
    }

    public override void Remove(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        if (managerData != null)
        {
            managerData.TakeDamageBullet(managerData.CurrentDamageBonus); // Revertir el bono en ManagerData 
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.damage -= managerData.CurrentDamageBonus; // Remover el bono actual
                }
            }
        }
        managerData.CurrentDamageBonus = 0; // Restablecer el bono
        Debug.Log("CurrentDamageBonuts " + managerData.CurrentDamageBonus);
    }
}
