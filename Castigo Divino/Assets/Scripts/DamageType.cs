using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageType
{
    void TakeDamage(float damage, BulletType bulletType); // Recibe el tipo de bala
}

public enum BulletType
{
    Fire,
    Water,
    Air 
}





