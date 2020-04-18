using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private CharacterController controller;
    public float speed = 5;
    public float maxHp = 50;

    public GameObject gfx;


    public Weapon weapon;

    [NonSerialized]
    public bool reloading;
    [NonSerialized]
    public int ammoLeft = 0;
    [NonSerialized]
    public float shootCooldown = 0f;
    [NonSerialized]
    public float hp = 0;

    private float actualSpeed;

    private Vector3 mousePoint;

    [NonSerialized]
    public Vector3 mousepointFlattened;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        hp = maxHp;
        ammoLeft = weapon.ammo;
        actualSpeed = speed;
    }

    private void Update()
    {
        actualSpeed = speed;
        

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(r, out hit);
        mousePoint = hit.point;
        mousepointFlattened = new Vector3(mousePoint.x, transform.position.y, mousePoint.z);
        gfx.transform.LookAt(mousepointFlattened,transform.up);

        if (Input.GetButton("Fire1"))
            Shoot();
        if (Input.GetButtonDown("Fire2"))
            Reload();

        UpdateGUI();

        Debug.Log("Actual Speed:" + actualSpeed);
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * actualSpeed;
        if (!controller.isGrounded) movement.y = -1; else movement.y = 0;
        controller.Move(movement);

        shootCooldown -= Time.deltaTime;
    }

    private void UpdateGUI()
    {
        GUIManager.instance.UpdateGUI();
    }

    private void Shoot()
    {
        Debug.Log("Shoot called");
        
        if(!reloading)
            actualSpeed = speed - weapon.speedDown;

        if (shootCooldown < 0)
        {
            shootCooldown = weapon.fireRate;
            Sound(10);
            Bullet.Shoot(transform.position, gfx.transform.localRotation, weapon, Bullet.BulletType.Friendly);
            reloading = false;
            ammoLeft--;
            if (ammoLeft <= 0)
            {
                Reload();
            }
        }

    }

    private void Reload()
    {
        if (!reloading)
        {
            Debug.Log("Reloading");
            shootCooldown += weapon.reloadTime;
            ammoLeft = weapon.ammo;
            reloading = true;
        }
    }

    public void Damage(float amount)
    {
        Debug.Log($"Player damaged by {amount} [{hp}/{maxHp}");
        hp -= amount;
    }

    public void Sound(float loudness)
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            int finalmask = ~((1 << 10) | (1 << 11)); //create layermask for player and enemy and invert it
            if (Vector3.Distance(enemy.transform.position,transform.position) <= loudness && !Physics.Linecast(transform.position, enemy.transform.position, finalmask))
            {
                enemy.GetComponent<Enemy>().lastNoiseLocation = transform.position;
                enemy.GetComponent<Enemy>().Heard();
            }
        }
    }
}
