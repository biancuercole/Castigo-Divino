using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PowerUps/BulletBuff")]
public class BulletBuff : PowerUpEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Bullets>().speed += amount;
    }
}
