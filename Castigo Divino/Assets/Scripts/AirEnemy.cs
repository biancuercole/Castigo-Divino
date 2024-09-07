using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : BaseEnemy
{
    public override void TakeDamage(float damage, BulletType bulletType)
    {
        // Personaliza el daño según el tipo de bala
        switch (bulletType)
        {
            case BulletType.Air:
                damage *= 0.5f; // Resistencia al aire
                break;
            case BulletType.Water:
                damage *= 2f; // Vulnerabilidad al agua
                break;
            case BulletType.Fire:
                damage *= 1f;
                break;
        }


        base.TakeDamage(damage, bulletType);
    }
}

