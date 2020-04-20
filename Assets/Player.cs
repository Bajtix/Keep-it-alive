using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public Transform head;
    public Transform spine;


    public Weapon weapon;

    public Animator animator;

    public Transform weaponHolder;

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
        Instantiate(weapon.model, weaponHolder);
    }

    private void Update()
    {
        actualSpeed = speed;
        

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(r, out hit);
        mousePoint = hit.point;
        mousepointFlattened = new Vector3(mousePoint.x, gfx.transform.position.y, mousePoint.z);
        gfx.transform.LookAt(new Vector3(
            gfx.transform.position.x + Input.GetAxis("Horizontal"),
            gfx.transform.position.y,
            gfx.transform.position.z + Input.GetAxis("Vertical")
            ));
        


        if(shootCooldown < 0.1f) animator.SetBool("Reload", false);
        if (!Input.GetButton("Fire1"))
            animator.SetBool("Aiming", false);

        if (Input.GetButton("Fire1"))
        {
            gfx.transform.LookAt(mousepointFlattened);
            Shoot();
        }
        if (Input.GetButtonDown("Reload"))
            Reload();   

        UpdateGUI();

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * actualSpeed;
        if (!controller.isGrounded) movement.y = -1; else movement.y = 0;
        controller.Move(movement);
        animator.SetFloat("Speed", Mathf.Abs(movement.x * 4 / Time.deltaTime) + Mathf.Abs(movement.z * 4 / Time.deltaTime));
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
            actualSpeed = speed - weapon.speedDown * Time.deltaTime / 0.03f;

        if (shootCooldown < 0)
        {
            animator.SetBool("Aiming", true);
            
            shootCooldown = weapon.fireRate;
            Utils.Sound(weapon.loudness,transform.position);
            Bullet.Shoot(weaponHolder.position, gfx.transform.rotation, weapon, Bullet.BulletType.Friendly);
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
        animator.SetBool("Aiming", false);
        animator.SetBool("Reload",true);
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

    

}
