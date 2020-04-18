using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject player;
    public GameObject bullet;
    public float attackDistance;
    public float stopDistance;
    public float angularSpeed;
    public float speed;
    public float maxHealth;


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
            Instantiate(bullet, transform.position, transform.rotation);
            coolDown = 0.2f / Time.deltaTime;
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
