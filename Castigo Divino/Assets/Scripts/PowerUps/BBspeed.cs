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
        if (managerData != null)
        {
            managerData.AddSpeedBullet(CurrentSpeedBonus);  // Aplica el bono en ManagerData
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.speed += CurrentSpeedBonus;  // Aplica el bono actual
                }
            }
        }
    }

    public override void Remove(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        if (managerData != null)
        {
            managerData.TakeSpeedBullet(CurrentSpeedBonus); // Revertir el bono en ManagerData
        }
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    bullet.speed -= CurrentSpeedBonus; // Remover el bono actual
                }
            }
        }
        CurrentSpeedBonus = 0; // Restablecer el bono
    }
}
