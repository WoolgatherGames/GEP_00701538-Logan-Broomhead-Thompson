using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HM_KeyboardKey : MonoBehaviour
{
    private HM_HackingManager _hackingManager;

    [SerializeField] KeyCode thisKey;

    private Image myImage;
    private bool currentlyActive;
    private float timeToDeactivate;

    bool spawnedThisFrame;//if the player has a really low frame rate, dont let the game spawn and despawn the key immediatly. 

    private void Start()
    {
        myImage = this.gameObject.GetComponent<Image>();
        myImage.enabled = false;
        _hackingManager = HM_HackingManager.instance;
    }

    public void ActivateKey(float time)
    {
        timeToDeactivate = time;
        currentlyActive = true;
        myImage.enabled = true;

        spawnedThisFrame = true;
    }

    void DeactivateKey()
    {
        currentlyActive = false;
        myImage.enabled = false;
    }

    private void Update()
    {
        if (currentlyActive)
        {
            if (Input.GetKeyDown(thisKey))
            {
                _hackingManager.KeyPressed();
                DeactivateKey();
            }

            timeToDeactivate -= Time.deltaTime;
            if (timeToDeactivate < 0f && !spawnedThisFrame)
                DeactivateKey();

            spawnedThisFrame = false;
        }
    }
}
