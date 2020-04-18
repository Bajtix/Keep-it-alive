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
        {
            if (Player.instance.shootCooldown > 0)
                ammo.text = $"Reloading... {Player.instance.shootCooldown.ToString("0.0")}s";
            else
                ammo.text = $"{Player.instance.ammoLeft}/{Player.instance.weapon.ammo}";
        }
    }
}
