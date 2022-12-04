using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_DetectionField : MonoBehaviour
{
    //This script looks for the player in a cone originating from the attatched object. 
    //This script needs to be attached to an EMPTY CHILD of whatever object needs it. It will rotate said object a bunch of times per frame very quickly
    //This script also requires a trigger volume infront of it, a decent bit larger than the actual detection FoV (the script will only shoot raycasts whilst the player is inside the trigger zone to save resources)
    //This script also needs to be in the "Ignore Raycast" layer. 

    //When the player enters a trigger field, charge for 0.5 seconds and if theyre still there. game over 
    //need to visualise to teh player where the camera / guard is actually looking 
    //use a polygon collider as the trigger. 
    //use this script for both cameras and guards, rather than re-doing it twice. 

    private float triggerTimer;//if the player is inside, increase this timer and if it hits 0.5 seconds. game over
    private bool playerInTrigger = false;

    [SerializeField] float detectionRange;
    [SerializeField] int detectionRadius;
    //[SerializeField] LayerMask myLayerMask;

    //When the player is close, this script will actually scan. otherwise its just wasting resources
    bool actuallyScan;

    private LayerMask detectionMask;
    private void Start()
    {
        //Physics2D.queriesHitTriggers = false;//im assuming this is global, and not specific to this script. but theres almost no documentation. anyway, could cause issues but i cant find a better way to ignore 2d trigger volumes
        actuallyScan = true;
        detectionMask = SM_StealthGameplayManager.instance.detectionFieldsLayerMask;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            actuallyScan = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerTimer = 0f;
            actuallyScan = false;
        }
    }

    private void Update()
    {
        //shoot raycasts out at different angles. Could disable this if we're far enough away from the player so as to save computing power
        //to do: make a check for how far the player is maybe...to see if i should run the camera code. 

        if (!actuallyScan)
            return;

        int timesPlayerHasBeenFoundThisFrame = 0;
        for (int x = -detectionRadius; x < detectionRadius; x++)
        {
            this.transform.localEulerAngles = new Vector3(0f, 0f, x + 90f);
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.up, detectionRange, detectionMask);
            Debug.DrawRay(this.transform.position, this.transform.up * detectionRange, Color.red, 0.1f, false);
            if (hit.collider != null)
                if (hit.collider.tag == "Player")
                    timesPlayerHasBeenFoundThisFrame += 1;
        }
        
        if (timesPlayerHasBeenFoundThisFrame > 0)
            playerInTrigger = true;
        else
            playerInTrigger = false;

        if (playerInTrigger)
        {
            triggerTimer += Time.deltaTime;
            if (triggerTimer > 0.25f)
            {
                //game over;
                Debug.Log("Game over. Player found!");
                GameManager.instance.GameOver();
            }
        }
        else
            triggerTimer = 0f;


    }
}


//Redundant Code: 
//Origonally used Trigger colliders during testing, not a viable permenant solution since the camera doesnt get stopped by walls
//Could possibly re-introduce a simialr method to check if the player is nearby. That way the camera doesnt need to shoot a million raycasts each second
/*
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
}*/

//I tried using some complicated maths, figuring out the co-ordinates of the point on a circle to calculate direction raycasts should shoot but it went pretty badly.
//Instead ive decided to have the game object rotate a million times per second. This solution would have been more elegant, and wouldnt have required rotating the game object this script is attatched too. 
/* 
int angleCount = -detectionRadius;
while (angleCount < detectionRadius)
{
    //RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.up, detectionRange);
    //the equasion for points on a circle is x = r * Cos(a), y = r * Sin(a) where A is the angle. 90 degrees is forward. e.g.If you plugged the radius of 1, and an angle of 90 into that equasion, itd be the same as transform.up
    //90 is the upwards direction (0,1), 180 is (-1,0), 270 (0,-1) and 0 or 360 is (0,1)
    Vector2 directionOfBeam = new Vector2(1f * Mathf.Cos(90 - angleCount), 1f * Mathf.Sin(90 - angleCount));
    angleCount += 1;

    RaycastHit2D hit = Physics2D.Raycast(this.transform.position, directionOfBeam, detectionRange);
    Debug.DrawRay(this.transform.position, directionOfBeam * detectionRange, Color.red, 0.1f, false);
    if (hit.collider != null)
        if (hit.collider.tag == "Player")
        {
            Debug.Log("I see you!!");
        }
}*/
