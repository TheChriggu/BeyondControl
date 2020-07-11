﻿using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.2f;
    public Vector3 offset;

    void Update()
    {
        Vector3 finalPosition = player.transform.position + offset;
        Vector3 currentPosition = Vector3.Lerp(transform.position, finalPosition, smoothSpeed);
        transform.position = currentPosition;
    }
}
