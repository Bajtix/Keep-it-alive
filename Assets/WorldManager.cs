using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using OdinSerializer;
using System.IO;

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

    public List<GameObject> dictionary;

    public Weapon[] tempWeaponArray;

    private void Awake()
    {
        rooms = new Room[1000,1000];
        if (instance != this)
        {
            Destroy(instance);
            instance = this;
        }
        else
            instance = this;

    }

    private void Start()
    {
        if (World.load)
            LoadWorld();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            SaveWorld();

        if (Input.GetKeyDown(KeyCode.G))
            LoadWorld();

        World.UpdateTime();
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
            r.AddObjectToRegister(Instantiate(selected, r.transform.position + randomPos, selected.transform.rotation)); 
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
            r.AddObjectToRegister(Instantiate(selected, r.transform.position + randomPos, selected.transform.rotation));
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
            r.AddObjectToRegister(Instantiate(selected, r.transform.position + randomPos, selected.transform.rotation));
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

    
    public void SaveWorld()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/world");
        string dataPath = Application.persistentDataPath + "/world/";
        foreach (Room room in rooms)
        {
            if (room != null)
            {
                if (!(room.myRoomX == 500 && room.myRoomY == 500))
                {
                    byte[] file = SerializationUtility.SerializeValue(room.Save(), DataFormat.JSON);
                    /*StreamWriter writer = new StreamWriter(dataPath + room.myRoomX + "_" + room.myRoomY + ".room");
                    writer.Write(file);
                    writer.Dispose();*/
                    File.WriteAllBytes(dataPath + room.myRoomX + "_" + room.myRoomY + ".room", file);
                }
            }
        }
        byte[] winfofile = SerializationUtility.SerializeValue(WorldStatus.GetCurrentStatus(), DataFormat.JSON);
        /*StreamWriter winfowriter = new StreamWriter(dataPath + "world.info");
        winfowriter.Write(winfofile);
        winfowriter.Dispose();*/
        File.WriteAllBytes(dataPath + "world.info", winfofile);
    }

    public void LoadWorld()
    {
        rooms[500, 500] = Room.starterRoom;
        
        string dataPath = Application.persistentDataPath + "/world/";
        foreach(string path in Directory.GetFiles(dataPath,"*.room"))
        {
            string roomPosTitle = Path.GetFileNameWithoutExtension(path);
            int posX = int.Parse(roomPosTitle.Split('_')[0]);
            int posY = int.Parse(roomPosTitle.Split('_')[1]);
            Room genr = Instantiate(roomPrefab, new Vector3((posX * 52) - 26000, 0, 40000 - (posY * 40) - 20000), Quaternion.identity).GetComponent<Room>();
            byte[] file = File.ReadAllBytes(path);
            
            Room.RoomInfo info = SerializationUtility.DeserializeValue<Room.RoomInfo>(file, DataFormat.JSON);
            WorldObject[] worldObjects = info.objs;
            genr.myRoomX = posX;
            genr.myRoomY = posY;
            genr.doors[0].physical.SetActive(info.doors[0]);
            genr.doors[1].physical.SetActive(info.doors[1]);
            genr.doors[2].physical.SetActive(info.doors[2]);
            genr.doors[3].physical.SetActive(info.doors[3]);
            rooms[posX, posY] = genr;

            
            foreach (WorldObject item in worldObjects)
            {             
                GameObject g = Instantiate(dictionary[item.id], new Vector3(item.x, item.y, item.z), new Quaternion(item.rotX, item.rotY, item.rotZ, item.rotW));
                genr.AddObjectToRegister(g);
                if (g.GetComponent<RandomRotation>() != null) Destroy(g.GetComponent<RandomRotation>());
            }
            byte[] winfofile = File.ReadAllBytes(dataPath + "world.info");
            WorldStatus status = SerializationUtility.DeserializeValue<WorldStatus>(winfofile, DataFormat.JSON);

            World.day = status.day;
            World.time = status.time;
            World.fuelInGenerator = status.fuel;
        }

        CameraController.instance.targetRoomPosition = Room.starterRoom.transform;
        CameraController.instance.roomPosition = Room.starterRoom.transform;
    }
}
