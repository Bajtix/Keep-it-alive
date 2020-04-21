using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public TextMeshProUGUI ammo;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI fuel;

    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI time;
    public TextMeshProUGUI day;

    public GameObject deathScreen;
    public TextMeshProUGUI deathMessage;
    public GameObject backButton;

    public Slider fuelLeft;

    public static GUIManager instance;

    public GameObject welcome;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ShowTooltip(string tooltip, Vector3 vector3)
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = tooltip;
        interactionText.transform.position = vector3;
    }
    public void HideTooltip()
    {
        interactionText.gameObject.SetActive(false);
    }
    public void Die(string msg)
    {
        deathScreen.SetActive(true);
        deathMessage.text = msg;
        if (msg == "Pause")
        {
            backButton.SetActive(true);
        }
        else
            backButton.SetActive(false);
    }

    public void HideDie()
    {
        deathScreen.SetActive(false);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void HideWelcome()
    {
        welcome.SetActive(false);
    }
    public void UpdateGUI()
    {
        if (!Player.instance.reloading)
            ammo.text = $"{Player.instance.ammoLeft} : {Player.instance.heldAmmo}";
        else
        {
            if (Player.instance.shootCooldown > 0)
                ammo.text = $"Reloading... {Player.instance.shootCooldown.ToString("0.0")}";
            else
                ammo.text = $"{Player.instance.ammoLeft}/{Player.instance.heldAmmo}";
        }

        if (Player.instance.hp > 0)
        {
            hp.text = $"{Mathf.RoundToInt(Player.instance.hp/10)}|{Mathf.RoundToInt(Player.instance.maxHp/10)} HP";
        }
        else
            hp.text = "Wait! You are dead!";

        fuel.text = Player.instance.fuelCarrying.ToString("0.0") + " L";

        time.text = "Day " + World.day + "\n" + Mathf.RoundToInt(World.time / 60).ToString("00") + ":" + Mathf.RoundToInt(World.time % 60).ToString("00");
        fuelLeft.value = World.fuelInGenerator;
    }
}
