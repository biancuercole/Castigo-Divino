using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    public abstract void Apply (GameObject target);
    public abstract void Remove(GameObject target);

    public float CurrentSpeedBonus { get; protected set; }
    public float CurrentDamageBonus { get; protected set; }
}
