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
    public GameObject bullet;
}
