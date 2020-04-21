using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : ShootReactor
{
    public int damage;
    public int range;

    public override void OnShoot()
    {
        Utils.Explode(transform.position, range, damage);
        Destroy(gameObject);
    }
}
