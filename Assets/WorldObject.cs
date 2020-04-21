using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject
{
    public int id;

    public float x;
    public float y;
    public float z;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    public WorldObject(int id, float x, float y, float z, float rotX, float rotY, float rotZ, float rotW)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.rotX = rotX;
        this.rotY = rotY;
        this.rotZ = rotZ;
        this.rotW = rotW;
    }
}
