using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private Dictionary<string, PowerUpEffect> activePowerUps = new Dictionary<string, PowerUpEffect>();

    public void ApplyPowerUp(PowerUpEffect powerUpEffect)
    {
        if (!activePowerUps.ContainsKey(powerUpEffect.name))
        {
            powerUpEffect.Apply(gameObject);
            activePowerUps.Add(powerUpEffect.name, powerUpEffect);
        }
    }

    public void RemovePowerUp(PowerUpEffect powerUpEffect)
    {
        if (activePowerUps.ContainsKey(powerUpEffect.name))
        {
            powerUpEffect.Remove(gameObject);
            activePowerUps.Remove(powerUpEffect.name);
        }
    }
}
