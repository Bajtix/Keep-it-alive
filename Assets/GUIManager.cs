using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public TextMeshProUGUI ammo;

    public static GUIManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void UpdateGUI()
    {
        if (!Player.instance.reloading)
            ammo.text = $"{Player.instance.ammoLeft}/{Player.instance.weapon.ammo}";
        else
            ammo.text = "RLD..";
    }
}
