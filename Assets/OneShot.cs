using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShot : Enemy
{
    public override void BattleMode()
    {
        int finalmask = ~((1 << 10) | (1 << 11)); //create layermask for player and enemy and invert it
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance && !Physics.Linecast(transform.position, player.transform.position, finalmask))
        {
            Attack();
        }
        if (this.reloading)
        {
            GoToNearestCover();
            actualSpeed = speed;          
        }
        else
        {           
            this.agent.destination = this.player.transform.position;
            agent.stoppingDistance = 6;
            actualSpeed = 1;
        }
        
        agent.updateRotation = false;
        LookAt();
    }

    private void GoToNearestCover()
    {
        agent.stoppingDistance = 1;
        GameObject closest = null;
        float lastDist = 99999;
        float newDist = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Env"))
        {
            newDist = Vector3.Distance(g.transform.position, transform.position);
            if (newDist < lastDist)
            {
                lastDist = newDist;
                closest = g;
            }
        }
        agent.destination = closest.transform.position;
    }
}
