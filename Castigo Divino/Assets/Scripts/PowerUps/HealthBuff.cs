using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthBuff")]
public class HealthBuff : PowerUpEffect
{
    public int amount;
    public override void Apply(GameObject target)
    {
       /* target.GetComponent<PlayerHealth>().IncreaseMaxHealth(amount);*/
    }
}
