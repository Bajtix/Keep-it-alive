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

    public GameObject gfx;


    public Weapon weapon;

    [NonSerialized]
    public bool reloading;
    [NonSerialized]
    public int ammoLeft = 0;
    [NonSerialized]
    public float shootCooldown = 0f;


    private Vector3 mousePoint;

    [NonSerialized]
    public Vector3 mousepointFlattened;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

        
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * speed;
        if (!controller.isGrounded) movement.y = -1; else movement.y = 0;
        controller.Move(movement);

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

        shootCooldown -= Time.deltaTime;
    }

    private void UpdateGUI()
    {
        GUIManager.instance.UpdateGUI();
    }

    private void Shoot()
    {
        Debug.Log("Shoot called");
        
        if (shootCooldown < 0)
        {
            shootCooldown = weapon.fireRate;
            Instantiate(weapon.bullet, transform.position, gfx.transform.localRotation);
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
}
