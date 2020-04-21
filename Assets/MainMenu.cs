using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueBtn;
    public TextMeshProUGUI weaponText;
    private void Start()
    {
        continueBtn.enabled = Directory.Exists(Application.persistentDataPath + "/world");
    }
    private void Update()
    {
        weaponText.text = "Selected weapon\n\n" + World.with.name;   
    }
    public void NewGame()
    {
        //Directory.Delete(Application.persistentDataPath + "/world", true);
        if (Directory.Exists(Application.persistentDataPath + "/world"))
        {
            foreach (string path in Directory.GetFiles(Application.persistentDataPath + "/world"))
            {
                File.Delete(path);
            }
            World.load = false;
            World.time = 360;
            World.day = 0;
            World.fuelInGenerator = 50;
        }
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {
        World.load = true;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
