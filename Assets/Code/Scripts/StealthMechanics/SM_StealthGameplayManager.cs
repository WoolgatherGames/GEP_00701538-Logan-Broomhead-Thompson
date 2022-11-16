using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_StealthGameplayManager : MonoBehaviour
{
    public static SM_StealthGameplayManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("You have too many stealth gameplay managers in the scene, my game object is called: " + this.gameObject.name);
        }
        else
            instance = this;
    }

    [Tooltip("This is what the guards / security cameras will look for. You want the player and the walls to be accesibile, the detection field script itself shouldn't be able to spot themselves")]
    public LayerMask detectionFieldsLayerMask;
}
