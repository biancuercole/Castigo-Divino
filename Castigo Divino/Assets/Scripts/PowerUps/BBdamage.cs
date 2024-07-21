using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/BulletBuffDamage")]
public class BulletBuffDamage : PowerUpEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        BulletPool bulletPool = BulletPool.Instance;
        if (bulletPool != null)
        {
            foreach (var bulletObject in bulletPool.bulletList)
            {
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                if (bullet != null)
                {
                    Debug.Log($"Bullet speed before: {bullet.damage}");
                    bullet.damage += amount;
                    Debug.Log($"Bullet speed after: {bullet.damage}");
                }
                else
                {
                    Debug.LogWarning("Bullet component not found on bullet object.");
                }
            }
        }
        else
        {
            Debug.LogWarning("BulletPool instance not found.");
        }
    }
}
