using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Bullet speed. Set by the weapon
    /// </summary>
    public float speed = 3f;
    /// <summary>
    /// Seconds before the bullet gets removed. Set by weapon
    /// </summary>
    public float lastTime = 1f;
    /// <summary>
    /// Bullet damage. Set by the weapon
    /// </summary>
    public float damage = 1f;

    private float time = 0f;

    public BulletType bulletType = BulletType.Enemy;

    public Light indicator;

    public enum BulletType
    {
        Friendly,
        Enemy
    }

    private void Start()
    {
        Apply();
    }

    public void Apply()
    {
        if (bulletType == BulletType.Enemy)
        {
            gameObject.layer = 9;
            indicator.color = Color.red;
        }
        else
        {
            gameObject.layer = 8;
            indicator.color = Color.green;
        }


    }

    private void Update()
    {
        time += Time.deltaTime;
        if (lastTime < time)
            Destroy(gameObject);
        transform.Translate(Vector3.forward * Time.deltaTime * speed,Space.Self);
        Utils.Sound(3, transform.position);
    }


    private void OnCollisionEnter(Collision collision)
    {

        Utils.Sound(3, transform.position);
        if (collision.collider.tag == "Enemy")
            collision.collider.GetComponent<Enemy>().Damaged(damage);

        if (collision.collider.tag == "Player")
            collision.collider.GetComponent<Player>().Damage(damage);

        Destroy(gameObject);
    }

    public static void Shoot(Vector3 position, Quaternion direction, Weapon from, BulletType bType)
    {
        for (int i = 0; i < from.bulletCount; i++)
        {
            Bullet b = Instantiate(from.bullet, position + new Vector3(0.1f, 0.1f, 0.1f) * Random.Range(-from.muzzleSize, from.muzzleSize), direction * Quaternion.Euler(new Vector3(0f, 10f, 0f) * Random.Range(-from.muzzleSize, from.muzzleSize))).GetComponent<Bullet>();
            b.damage = from.bulletDamage;
            b.speed = from.bulletSpeed;
            b.lastTime = from.bulletLastTime;
            b.bulletType = bType;
            b.Apply();
        }       
    }
}
