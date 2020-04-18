using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5;

    public GameObject gfx;
    public GameObject bullet;

    private Vector3 mousePoint;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * speed;
        controller.Move(movement);

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(r, out hit);
        mousePoint = hit.point;
        gfx.transform.LookAt(mousePoint,Vector3.up);
    }
}
