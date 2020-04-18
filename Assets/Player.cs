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
    public GameObject bullet;

    private Vector3 mousePoint;
    [System.NonSerialized]
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
        
        if(Input.GetButtonDown("Fire1"))
        if(/*hit.collider.gameObject.tag == "Enemy"*/ true)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, transform.position, gfx.transform.localRotation);
    }
}
