using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utils : MonoBehaviour
{
    public static void Sound(float loudness, Vector3 position)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            int finalmask = ~((1 << 10) | (1 << 11)); //create layermask for player and enemy and invert it
            if (Vector3.Distance(enemy.transform.position, position) <= loudness)
            {
                float chance = 0;
                if (!Physics.Linecast(position, enemy.transform.position, finalmask))
                {
                    chance = 1;
                }
                else
                {
                    chance = 0.3f;
                }
                if (Random.Range(0, 1) < chance)
                {
                    enemy.GetComponent<Enemy>().lastNoiseLocation = position;
                    enemy.GetComponent<Enemy>().Heard();
                }
            }
        }
    }

    public static void Explode(Vector3 position, int range, int damage)
    {
        CameraController.instance.CameraShake(1/Time.deltaTime,1);
        Instantiate(WorldManager.instance.explosionEffect, position, Quaternion.identity);
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(Vector3.Distance(position,g.transform.position) <= range)
            {
                g.GetComponent<Enemy>().Damaged(damage);
            }
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Vector3.Distance(position, g.transform.position) <= range)
            {
                g.GetComponent<Player>().Damage(damage);
            }
        }

        foreach (Collider g in Physics.OverlapSphere(position,range))
        {
            if(g.GetComponent<Rigidbody>() != null)
            {
                g.GetComponent<Rigidbody>().AddExplosionForce(damage * 100, position, range);
            }
        }
    }
}
