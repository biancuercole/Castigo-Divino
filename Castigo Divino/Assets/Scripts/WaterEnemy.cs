using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterEnemy : BaseEnemy
{
    public override void TakeDamage(float damage, BulletType bulletType)
    {
        switch (bulletType)
        {

            case BulletType.Fire:
                damage *= 0.5f;
                break;
            case BulletType.Water:
                damage *= 0.5f; 
                break;
            case BulletType.Air:
                damage *= 2f;
                break;
            case BulletType.Earth:
                damage *= 1f;
                break;
            case BulletType.GodPower:
                damage *= 1f;
                break;
        }
 
        base.TakeDamage(damage, bulletType);
    }
}

