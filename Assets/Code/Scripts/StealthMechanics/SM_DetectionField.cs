using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_DetectionField : MonoBehaviour
{
    //When the player enters a trigger field, charge for 0.5 seconds and if theyre still there. game over 
    //need to visualise to teh player where the camera / guard is actually looking 
    //use a polygon collider as the trigger. 
    //use this script for both cameras and guards, rather than re-doing it twice. 

    private float triggerTimer;//if the player is inside, increase this timer and if it hits 0.5 seconds. game over
    private bool playerInTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("boop");
        if (collision.transform.tag == "Player")
        {
            triggerTimer = 0f;
            playerInTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerInTrigger = false;
        }
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            triggerTimer += Time.deltaTime;
            if (triggerTimer > 0.5f)
            {
                //game over;
                Debug.Log("Game over. Player found!");
            }
        }
    }
}
