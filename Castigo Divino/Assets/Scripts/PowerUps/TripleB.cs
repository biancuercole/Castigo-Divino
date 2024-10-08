using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/TripleBullet")]

public class TripleB : PowerUpEffect
{

    public override void Apply(GameObject target)
    {
        Rotation rotation = target.GetComponentInChildren<Rotation>();
        if (rotation != null)
        {
            rotation.EnableTripleShot();
            //Debug.Log("Triple shot enabled"); // Agregar este mensaje de depuraci�n
        }
        else
        {
            //Debug.LogWarning("Rotation component not found on target.");
        }
    }
    public override void Remove(GameObject target)
    {
        Rotation rotation = target.GetComponentInChildren<Rotation>();
        if (rotation != null)
        {
            rotation.DisableTripleShot();
        }
        else
        {
            //Debug.LogWarning("Rotation component not found on target.");
        }
    }
}
