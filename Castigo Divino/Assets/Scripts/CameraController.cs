using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private float followSpeed;
    [SerializeField]private Transform target;
    [SerializeField]private float yOffset; 

    void Update()
    {
        Vector3 newSpot = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newSpot, followSpeed * Time.deltaTime); 
    }
}
