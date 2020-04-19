using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO: Actual camera controls.

    public static CameraController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public Transform tracked;
    public Transform roomPosition;
    public Transform targetRoomPosition;
    public Vector3 offset;
    public Vector3 multiplication;

    float lerp = 0;
    public float speed = 10;

    private void Update()
    {
        Vector3 pos = Vector3.Lerp(roomPosition.position, targetRoomPosition.position, lerp);
        if(targetRoomPosition.position != roomPosition.position)
        {
            lerp += speed * Time.deltaTime;
        }
        if(lerp >= 1)
        {
            roomPosition = targetRoomPosition;
            lerp = 0;
        }
        transform.position = pos + ((tracked.position - pos) * 0.5f) + offset;
    }
}
