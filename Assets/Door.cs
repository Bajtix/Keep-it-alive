using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 roomOffset;
    public int direction;
    public GameObject physical;
    public Room parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<Room>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            WorldManager.instance.CreateRoom(transform.position + roomOffset, direction,parent.myRoomX,parent.myRoomY);
        }
    }
}
