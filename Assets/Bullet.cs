using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Bullet speed
    /// </summary>
    public float speed = 3f;
    /// <summary>
    /// Seconds before the bullet gets removed
    /// </summary>
    public float lastTime = 1f;
    /// <summary>
    /// Bullet damage
    /// </summary>
    public float damage = 1f;

    private float time = 0f;

    

    private void Update()
    {
        time += Time.deltaTime;
        if (lastTime < time)
            Destroy(gameObject);
        transform.Translate(Vector3.forward * Time.deltaTime * speed,Space.Self);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision entered");
        if (collision.collider.tag == "Enemy")
            collision.collider.GetComponent<Enemy>().Damaged(damage);

        Destroy(gameObject);
    }
}
