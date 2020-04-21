using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : Interaction
{
    public override void Interact()
    {
        if (Player.instance.fuelCarrying > 0)
        {
            useTime = Player.instance.fuelCarrying / 4;
            Player.instance.fuelCarrying -= Time.deltaTime * 4;
            World.fuelInGenerator += Time.deltaTime * 4;
        }
        else
            Done();
    }
}
