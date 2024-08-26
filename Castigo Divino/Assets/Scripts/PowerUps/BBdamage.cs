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
        if (managerData != null)
        {
            managerData.AddDamageBullet(CurrentDamageBonus);  // Aplica el bono en ManagerData 
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.damage += CurrentDamageBonus;  // Aplica el bono actual
                }
            }
        }
    }

    public override void Remove(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        if (managerData != null)
        {
            managerData.TakeDamageBullet(CurrentDamageBonus); // Revertir el bono en ManagerData 
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.damage -= CurrentDamageBonus; // Remover el bono actual
                }
            }
        }
        CurrentDamageBonus = 0; // Restablecer el bono
    }
}
