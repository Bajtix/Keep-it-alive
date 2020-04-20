using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [NonSerialized]
    public NavMeshAgent agent;
    [NonSerialized]
    public GameObject player;
    public Weapon weapon;
    public float attackDistance;
    public float stopDistance;
    public float angularSpeed;
    public float speed;
    public float maxHealth;
    public float aimSpeed = 2;
    public float aimPrecission = 1;
    public float viewDistance = 20;
    public float FOV = 60;

    public GameObject ragdoll;

    public Transform weaponPos;

    [NonSerialized]
    public Vector3 lastNoiseLocation;

    private int ammo = 0;
    private float coolDown;
    [NonSerialized]
    public float health;
    [NonSerialized]
    public float actualSpeed;
    [NonSerialized]
    public bool reloading;

    private bool alive = true;

    public AttentionStatus status = AttentionStatus.Wander;

    public enum AttentionStatus
    {
        Battle,
        Wander,
        Attention
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDistance;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        gameObject.layer = 10;
        health = maxHealth;
        ammo = weapon.ammo;
        player = Player.instance.gameObject;
        Instantiate(weapon.model, weaponPos);

    }

    private void Update()
    {
        lookDirection = transform.eulerAngles.y * Mathf.Deg2Rad;
        agent.speed = actualSpeed;

        if (status == AttentionStatus.Wander)
            WanderMode();

        if (status == AttentionStatus.Battle)
            BattleMode();

        if (status == AttentionStatus.Attention)
            AttentionMode();

        coolDown -= Time.deltaTime;
    }

    public virtual void Attack()
    {
        if (!reloading)
            actualSpeed = speed - weapon.speedDown;

        if (coolDown < 0)
        {
            reloading = false;
            Bullet.Shoot(transform.position, transform.localRotation, weapon, Bullet.BulletType.Enemy);
            Utils.Sound(weapon.loudness, transform.position);
            ammo--;
            coolDown = weapon.fireRate;

            if(ammo <= 0)
            {
                reloading = true;
                coolDown = weapon.reloadTime;
                ammo = weapon.ammo;
            }
        }
    }


    private float sign(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1.x - p3.x) * (p2.z - p3.z) - (p2.x - p3.x) * (p1.z - p3.z);
    }

    private bool IsPointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = sign(pt, v1, v2);
        d2 = sign(pt, v2, v3);
        d3 = sign(pt, v3, v1);

        has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }
    float lookDirection = 0;
    public virtual void WanderMode()
    {
        actualSpeed = speed / 2;
        agent.updateRotation = true;
        if (agent.velocity == Vector3.zero)
            agent.SetDestination(transform.position + new Vector3(7 * Random.Range(-1f, 1.1f), 0, 7 * Random.Range(-1f, 1.1f)) );


        

        int finalmask = ~((1 << 10) | (1 << 11)); //create layermask for player and enemy and invert it
        if (Vector3.Distance(transform.position, player.transform.position) <= viewDistance && !Physics.Linecast(transform.position, player.transform.position, finalmask))
        {
            //Vector3 targetDir = player.transform.position - transform.position;
            //float angle = Mathf.Rad2Deg * Mathf.Atan(targetDir.x / targetDir.z);
            //Debug.Log("angle: " + angle + "ENEMY ANGLE " + transform.rotation.eulerAngles.y);
            
            Vector3 point0 = transform.position;
            Vector3 point1 = transform.position + (viewDistance * new Vector3(-Mathf.Cos(FOV + lookDirection), 0, Mathf.Sin(FOV + lookDirection)));
            Vector3 point2 = transform.position + (viewDistance * new Vector3(Mathf.Cos(FOV - lookDirection), 0, Mathf.Sin(FOV - lookDirection)));
            if (IsPointInTriangle(player.transform.position, point0, point1, point2))
                status = AttentionStatus.Battle;


        }

        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 targetDir = player.transform.position - transform.position;
        //Gizmos.DrawLine(transform.position, transform.position + new Vector3(targetDir.x,0,0));
        //Gizmos.DrawLine(transform.position + new Vector3(targetDir.x, 0, 0), transform.position + new Vector3(targetDir.x, 0, 0) + new Vector3(0,0,targetDir.z));
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
        //Gizmos.DrawLine(transform.position, player.transform.position);


        Gizmos.DrawLine(transform.position, transform.position + (viewDistance * new Vector3(Mathf.Cos(FOV - lookDirection),0,Mathf.Sin(FOV - lookDirection))));
        Gizmos.DrawLine(transform.position, transform.position + (viewDistance * new Vector3(-Mathf.Cos(FOV + lookDirection),0,Mathf.Sin(FOV + lookDirection))));
        
    }
    public virtual void BattleMode()
    {

        actualSpeed = speed;
        int finalmask = ~((1 << 10) | (1 << 11)); //create layermask for player and enemy and invert it
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance && !Physics.Linecast(transform.position,player.transform.position,finalmask))
        {
            Attack();
        }
        agent.SetDestination(player.transform.position);
                
        agent.updateRotation = false;
        LookAt();
    }

    public virtual void AttentionMode()
    {
        actualSpeed = speed;
        agent.updateRotation = true;
        agent.SetDestination(lastNoiseLocation + new Vector3(Random.Range(-3f,3f),0, Random.Range(-3f,3f)));

        int finalmask = ~((1 << 10) | (1 << 11)); //create layermask for player and enemy and invert it
        if (Vector3.Distance(transform.position, player.transform.position) <=  viewDistance * 2 && !Physics.Linecast(transform.position, player.transform.position, finalmask))
        {
            status = AttentionStatus.Battle;
        }
    }

    public virtual void Heard()
    {
        if(status != AttentionStatus.Battle)
            status = AttentionStatus.Attention;
    }
    
    public virtual void Damaged(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Debug.Log("Death");
            if(alive)
                Instantiate(ragdoll, transform.position, transform.rotation);
            alive = false;
            if (GetComponent<EnemyGUI>() != null)
                Destroy(GetComponent<EnemyGUI>().ui);
            Destroy(gameObject);
        }

        Debug.Log($"Damaged: {amount}hp [{health}/{maxHealth}]");
    }

    public void SpawnRagdollOfSelf()
    {

    }

    public void LookAt()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized + new Vector3(0.1f, 0.0f, 0.1f) * Random.Range(-aimPrecission, aimPrecission);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);
    }
}
