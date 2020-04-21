using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeapon : MonoBehaviour
{
    public Weapon w;
    public GameObject[] pn;
    public int pid;
    public Weapon shotgun;


    private void Start()
    {
        if(shotgun != null)
        World.with = shotgun;
    }
    private void OnMouseOver()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            World.with = w;
            try
            {
                pn[0].SetActive(false);
                pn[1].SetActive(false);
                pn[2].SetActive(false);

                pn[pid].SetActive(true);
            }
            catch
            {
                Debug.Log("An error occurred, but it's fine");
            }
        }
    }
}
