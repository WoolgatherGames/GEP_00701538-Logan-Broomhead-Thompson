using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HM_HackableObject : MonoBehaviour
{
    [SerializeField] Difficulty hackingDifficulty;
    bool hasBeenHacked;

    private void Start()
    {
        hasBeenHacked = false;
    }

    public void HackInteract()
    {
        //activate when the player clicks on this object
        if (hasBeenHacked)
        {
            //if the object has been hacked, enable its functionality
        }
        else
        {
            //hack the object
            HM_HackingManager.instance.BeginHack(hackingDifficulty);
        }
    }

    private void OnMouseDown()
    {
        HackInteract();
    }
}
