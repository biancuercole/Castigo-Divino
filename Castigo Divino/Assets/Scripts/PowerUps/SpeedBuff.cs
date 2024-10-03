using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]

public class SpeedBuff : PowerUpEffect
{
    public float amount;

    public ManagerData managerData;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerMovement>().speed += amount;
        managerData = FindObjectOfType<ManagerData>();
        managerData.AddSpeed(amount);
        //.Log("Incresed speed");
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<PlayerMovement>().speed -= amount;
    }
}
