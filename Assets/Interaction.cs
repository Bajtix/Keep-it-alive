using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float maxDistance = 50;
    public string tooltip = "[ RMB ] Interact";
    public float useTime = 1;
    public float soundLoudness;
    
    public void Tooltip()
    {
        GUIManager.instance.ShowTooltip(
                tooltip + " - " + useTime.ToString("0.0") + "s",
                Camera.main.WorldToScreenPoint(
                    transform.position + new Vector3(0, 1, 0)
                    )
                );
    }

    public virtual void Interact()
    {
        useTime -= Time.deltaTime;
        if (useTime <= 0)
            Done();
    }

    public virtual void Done()
    {
        Utils.Sound(soundLoudness, transform.position);
    }
}
