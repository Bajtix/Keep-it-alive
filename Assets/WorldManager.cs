using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    public Room[,] rooms;

    public GameObject roomPrefab;

    public GameObject[] spawningObjects;
    public GameObject[] wallObjects;
    public GameObject[] enemyObjects;
    public GameObject[] obstacleObjects;

    public int objectDensity;
    public int wallObjectDensity;

    public GameObject explosionEffect;

    private void Awake()
    {
        rooms = new Room[1000,1000];
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }

    public void CreateRoom(Vector3 position,int direction, int myRoomX, int myRoomY)
    {

        int NmyRoomX = 0, NmyRoomY = 0;
        
        switch (direction)
        {
            case 0:
                NmyRoomX = myRoomX + 1;
                NmyRoomY = myRoomY;
                Debug.Log("ROOM DATA:" + myRoomX + "; " + myRoomY);
                break;
            case 1:
                NmyRoomX = myRoomX - 1;
                NmyRoomY = myRoomY;
                Debug.Log("ROOM DATA:" + myRoomX + "; " + myRoomY);
                break;
            case 2:
                NmyRoomX = myRoomX;
                NmyRoomY = myRoomY - 1;
                Debug.Log("ROOM DATA:" + myRoomX + "; " + myRoomY);
                break;
            case 3:
                NmyRoomX = myRoomX;
                NmyRoomY = myRoomY + 1;
                Debug.Log("ROOM DATA:" + myRoomX + "; " + myRoomY);
                break;
        }
        Room r = null;
        if (rooms[NmyRoomX, NmyRoomY] == null)
        {
            Debug.Log("ROOM AT:" + myRoomX + "; " + myRoomY + " is null. Filling it in!");
            r = Instantiate(roomPrefab, position, Quaternion.identity).GetComponent<Room>();
            r.Generate(direction, NmyRoomX, NmyRoomY);
            CameraController.instance.targetRoomPosition = rooms[myRoomX, myRoomY].transform;
            rooms[NmyRoomX, NmyRoomY] = r;
            
        }
        else
        {
            CameraController.instance.targetRoomPosition = rooms[myRoomX, myRoomY].transform;  
        }

        
        
        if (rooms[NmyRoomX + 1, NmyRoomY] != null)
        {
            Debug.Log("Nieghbour rooms found");
            Room w = rooms[NmyRoomX + 1, NmyRoomY];
            Doors(w, ref r, 1);
        }
        if (rooms[NmyRoomX - 1, NmyRoomY] != null)
        {
            Debug.Log("Nieghbour rooms found");
            Room w = rooms[NmyRoomX - 1, NmyRoomY];
            Doors(w, ref r, 0);
        }
        if (rooms[NmyRoomX, NmyRoomY + 1] != null)
        {
            Debug.Log("Nieghbour rooms found");
            Room w = rooms[NmyRoomX, NmyRoomY + 1];
            Doors(w, ref r, 2);
        }
        if (rooms[NmyRoomX, NmyRoomY - 1] != null)
        {
            Debug.Log("Nieghbour rooms found");
            Room w = rooms[NmyRoomX, NmyRoomY - 1];
            Doors(w, ref r, 3);
        }

        GenFloorObjects(r);
        GenWallDecor(r);
        SpawnEnemies(r);
        GenObstacleObjects(r);
    }

    private void GenFloorObjects(Room r)
    {
        for (int i = 0; i < objectDensity; i++)
        {
            GameObject selected = null;
            while(selected == null)
                selected = spawningObjects[Random.Range(0, spawningObjects.Length)];
            Vector3 randomPos = new Vector3(
                Random.Range(-20f,20f),
                0,
                Random.Range(-12F, 12f)
                );
            if (r == null) return;
            Instantiate(selected, r.transform.position + randomPos, selected.transform.rotation); 
        }
    }

    private void GenObstacleObjects(Room r)
    {
        int randomCount = Random.Range(0,5);
        for (int i = 0; i < randomCount; i++)
        {
            GameObject selected = null;
            while (selected == null)
                selected = obstacleObjects[Random.Range(0, obstacleObjects.Length)];
            Vector3 randomPos = new Vector3(
                Random.Range(-20f, 20f),
                0,
                Random.Range(-12F, 12f)
                );
            if (r == null) return;
            Instantiate(selected, r.transform.position + randomPos, selected.transform.rotation);
        }
    }

    private void SpawnEnemies(Room r)
    {
        int randomCount = Random.Range(0, 10);
        for (int i = 0; i < randomCount; i++)
        {
            GameObject selected = null;
            while (selected == null)
                selected = enemyObjects[Random.Range(0, enemyObjects.Length)];
            Vector3 randomPos = new Vector3(
                Random.Range(-20f, 20f),
                0,
                Random.Range(-12F, 12f)
                );
            if (r == null) return;
            Instantiate(selected, r.transform.position + randomPos, selected.transform.rotation);
        }
    }

    private void GenWallDecor(Room r)
    {
        
        for (int i = 0; i < wallObjectDensity; i++)
        {
            GameObject selected = null;
            while (selected == null)
                selected = wallObjects[Random.Range(0, wallObjects.Length)];
            if (r == null) return;
            Transform randomPos = r.wallDecors[Random.Range(0, r.wallDecors.Count)];
            if(selected != null && randomPos.childCount == 0)
            Instantiate(selected, randomPos);
        }       
    }

    void Doors(Room w, ref Room r, int dir)
    {
        if (r == null) return;
        if (dir == 1)
        {
            if (!w.doors[0].physical.activeSelf)
            {
                r.doors[1].physical.SetActive(false);
            }
            else
            {
                r.doors[1].physical.SetActive(true);
            }
        }

        if (dir == 0)
        {
            if (!w.doors[1].physical.activeSelf)
            {
                r.doors[0].physical.SetActive(false);
            }
            else
            {
                r.doors[0].physical.SetActive(true);
            }
        }

        if (dir == 3)
        {
            if (!w.doors[2].physical.activeSelf)
            {
                r.doors[3].physical.SetActive(false);
            }
            else
            {
                r.doors[3].physical.SetActive(true);
            }
        }

        if (dir == 2)
        {
            if (!w.doors[3].physical.activeSelf)
            {
                r.doors[2].physical.SetActive(false);
            }
            else
            {
                r.doors[2].physical.SetActive(true);
            }
        }
    }
}
