﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    [NonSerialized]
    public bool[] paths = new bool[4];

    public Door[] doors;

    public int myRoomX = 50, myRoomY = 50;

    public bool isStarter = false;

    public List<Transform> wallDecors;

    private void Awake()
    {
        wallDecors = new List<Transform>();
        for (int i = 0; i < transform.Find("Wall Decor").childCount; i++)
        {
            wallDecors.Add(transform.Find("Wall Decor").GetChild(i));
            Debug.Log("Added decor slot #1");
        }
    }
    private void Start()
    {
        
        if (isStarter)
        {
            WorldManager.instance.rooms[myRoomX, myRoomY] = this;
            Debug.Log("Starter room set");
            doors[3].physical.SetActive(false);
        }

        

        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void Generate(int incomingDir, int mx, int my)
    {
        myRoomX = mx;
        myRoomY = my;
        //paths[incomingDir] = true;

        for (int i = 0; i < 4; i++)
        {
            if (paths[i] != true)
                paths[i] = Random.Range(0, 3) < 2;

            if (paths[i]) 
            { 
                doors[i].physical.SetActive(false); 
                //Destroy(doors[i].gameObject); 
            }
        }
    }
}
