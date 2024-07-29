using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]

public class SpeedBuff : PowerUpEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerMovement>().speed += amount;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<PlayerMovement>().speed -= amount;
    }
}
