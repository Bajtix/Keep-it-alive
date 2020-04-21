using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCanister : Interaction
{

    public override void Interact()
    {
        base.Interact();
        Utils.Sound(soundLoudness / 2, transform.position);
    }

    public override void Done()
    {
        base.Done();
        Player.instance.fuelCarrying += 20;
        Destroy(gameObject);
    }
}
