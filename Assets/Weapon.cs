using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/WEAPON")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int ammo;
    public float fireRate;
    public float reloadTime;
    public float speedDown;
    public float muzzleSize;
    public int bulletCount = 1;
    public float loudness;

    public float bulletSpeed;
    public float bulletDamage;
    public float bulletLastTime;

    public GameObject bullet;

    public GameObject model;

    public int id;
}
