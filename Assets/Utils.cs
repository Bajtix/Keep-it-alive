using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
