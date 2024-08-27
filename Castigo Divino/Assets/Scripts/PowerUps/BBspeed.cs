using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/BulletBuffSpeed")]
public class BBspeed : PowerUpEffect
{
    public float amount;

    public ManagerData managerData;

    public override void Apply(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        managerData = FindObjectOfType<ManagerData>();
        if (managerData != null)
        {
            managerData.AddSpeedBullet(amount - managerData.CurrentSpeedBonus); // Ajustar la velocidad según el nuevo bono
            Debug.Log("Damage Bonus " + managerData.CurrentSpeedBonus + "Amount " + amount);
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.speed -= managerData.CurrentSpeedBonus; // Revertir cualquier bono anterior
                    bullet.speed += amount; // Aplicar el nuevo bono
                }
            }
        }
        managerData.CurrentSpeedBonus = amount; // Guardar el bono actual
        Debug.Log("CurrentSpeedBonuts " + managerData.CurrentSpeedBonus);
    }

    public override void Remove(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        if (managerData != null)
        {
            managerData.TakeSpeedBullet(managerData.CurrentSpeedBonus); // Revertir el bono en ManagerData
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.speed -= managerData.CurrentSpeedBonus; // Remover el bono actual
                }
            }
        }
        managerData.CurrentSpeedBonus = 0; // Restablecer el bono
        Debug.Log("CurrentSpeedBonuts " + managerData.CurrentSpeedBonus);
    }
}
