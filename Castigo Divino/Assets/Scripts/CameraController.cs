using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float yOffset;
    [SerializeField] private float xOffset;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = target.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float offset = playerMovement.IsFacingRight ? xOffset : -xOffset;
        Vector3 newSpot = new Vector3(target.position.x + offset, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newSpot, followSpeed * Time.deltaTime);
    }
}
