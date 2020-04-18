using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject player;
    public Weapon weapon;
    public float attackDistance;
    public float stopDistance;
    public float angularSpeed;
    public float speed;
    public float maxHealth;

    private int ammo = 0;
    private float coolDown;
    [System.NonSerialized]
    public float health;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDistance;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        gameObject.layer = 10;
        health = maxHealth;
    }

    private void Update()
    {
        Updates();
        agent.SetDestination(player.transform.position);
        if(Vector3.Distance(transform.position,player.transform.position) <= attackDistance)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        coolDown--;
        if (coolDown < 0)
        {
            Bullet.Shoot(transform.position, transform.localRotation, weapon, Bullet.BulletType.Enemy);
            ammo--;
            coolDown = weapon.fireRate / Time.deltaTime;

            if(ammo <= 0)
            {
                coolDown = weapon.reloadTime;   
            }
        }
    }

    public virtual void Updates()
    {

    }

    public virtual void Damaged(float amount)
    {
        health -= amount;
        if (health <= 0)
            Destroy(gameObject);

        Debug.Log($"Damaged: {amount}hp [{health}/{maxHealth}]");
    }
}
