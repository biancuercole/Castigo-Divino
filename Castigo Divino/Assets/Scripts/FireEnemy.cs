using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : BaseEnemy
{
    public override void TakeDamage(float damage, BulletType bulletType)
    {
        switch (bulletType)
        {
            case BulletType.Fire:
                damage *= 0.5f; // Resistencia al fuego
                break;
            case BulletType.Water:
                damage *= 2f; // Vulnerabilidad al agua
                break;
            case BulletType.Air:
                damage *= 1f;
                break;
        }

        base.TakeDamage(damage, bulletType);
    }
}
