using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyGUI : MonoBehaviour
{
    public Sprite surprised, battlemode, aim, reload, uiMask;
    private Enemy myEnemy;
    public GameObject prefab;
    public GameObject canvas;
    [NonSerialized]
    public GameObject ui;
    private Image statusImg;
    
    void Start()
    {
        myEnemy = GetComponent<Enemy>();
        if (canvas == null)
            canvas = GameObject.Find("Canvas");
        ui = Instantiate(prefab,canvas.transform);
        statusImg = ui.GetComponent<Image>();
        
    }

    void Update()
    {
        ui.transform.position = Camera.main.WorldToScreenPoint(myEnemy.transform.position + new Vector3(0,4,0));
        if (myEnemy.status != Enemy.AttentionStatus.Wander)
        {
            if (myEnemy.reloading)
            {
                statusImg.sprite = reload;
            }
            else
            {
                if (myEnemy.status == Enemy.AttentionStatus.Battle)
                {
                    statusImg.sprite = battlemode;
                }
                else if (myEnemy.status == Enemy.AttentionStatus.Attention)
                {
                    statusImg.sprite = surprised;
                }
                else
                    statusImg.sprite = aim;
            }
        }
        else
            statusImg.sprite = uiMask;
    }
}
