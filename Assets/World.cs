using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World
{
    public static float time = 360; //A night is 720 and a day is 720. //Day: 6 - 18, night 18 - 6
    public static int day = 0;
    public static float fuelInGenerator = 50;
    public static bool load = false;
    public static Weapon with;
    public static void UpdateTime()
    {
        time += Time.deltaTime * 2; //1 minute = 0.5 second
        if(time >= 1440)
        {
            day++;
            time = 0;
        }
        fuelInGenerator -= Time.deltaTime / 6;
        if (fuelInGenerator < 1)
        {
            GUIManager.instance.Die("Fuel out!");
            Time.timeScale = 0.001f;
        }
            
    }
}

public class WorldStatus
{
    public int weapon;
    public float time;
    public int day;
    public float fuel;
    public float hp;

    public WorldStatus(int weapon, float time, int day, float fuel, float hp)
    {
        this.weapon = weapon;
        this.time = time;
        this.day = day;
        this.fuel = fuel;
        this.hp = hp;
    }

    public static WorldStatus GetCurrentStatus()
    {
        return new WorldStatus(0,World.time,World.day,World.fuelInGenerator, 0);
    }
}
