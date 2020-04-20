using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private void Start()
    {
        transform.Rotate(Vector3.forward, Random.Range(0, 360));
    }
}
