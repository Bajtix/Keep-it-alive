using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interaction
{
    public int minAmmo, maxAmmo;
    public override void Done()
    {
        base.Done();
        Player.instance.heldAmmo += Random.Range(minAmmo, maxAmmo);
        Destroy(gameObject);
    }


}
