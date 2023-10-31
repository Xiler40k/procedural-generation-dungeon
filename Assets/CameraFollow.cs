using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset = new Vector3(0, 0, -10);
    private Vector3 velocity = Vector3.zero;
    public float cameraDelay;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, cameraDelay);
    }
}