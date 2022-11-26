using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_PatrollingGuard : MonoBehaviour
{
    public enum Behaviours
    {
        patrolRotate,//the guard rotates to face their next patrol location
        patrolWalk,//the guard walks between patrol locations
        investigate,//The guard has finished walking and should scan around a little bit (maybe?) was going to be when the guard hears a sound and investigates it but that seems like itll take a lot of time. 
    };

    Behaviours currentBehaviour = Behaviours.patrolRotate;

    [Tooltip("The world locations of where this guard will move between. They will move in order. The guard will walk in STRAIGHT LINES. make sure theres no walls in their route. Keep the Z axis clear")]
    [SerializeField] Vector3[] patrolLocations;

    //whilst true, once the guard reaches a patrol location. they will move to the nxt once. whilst false, they will move down the patrol locations instead. 
    bool headingUpPatrolRoute;
    //the current number of patrol location that the guard is heading towards
    int currentPatrol;
    //the location that the guard is heading towards
    Vector3 nextLocation;

    //is the guard currently rotating. Whilst rotating, they shouldn't move.  
    //bool rotating; (no longer used as I'm using a switch instead. 

    //the tramsform of the guard
    Transform myTransform;
    //When the guard is in rotation behavior mode, what is their desired rotation (should be a value between -180 and 180)
    float rotationGoal;

    void Start()
    {
        myTransform = this.gameObject.GetComponent<Transform>();
        //Set the guards starting location
        myTransform.position = patrolLocations[0];

        //set first location goal, then point the guard towards it
        headingUpPatrolRoute = true;
        currentPatrol = 0;
        nextLocation = patrolLocations[currentPatrol+1];//if there isnt two patrol locations this will cause a bug, but a guard should have atleast two patrol locations

        //https://answers.unity.com/questions/1023987/lookat-only-on-z-axis.html found this online as a replacement to LookAt() for 2d. 
        Vector3 differenceBetweenLocations = nextLocation - myTransform.position;
        //rotationGoal = Mathf.Atan2(differenceBetweenLocations.y, differenceBetweenLocations.x) * Mathf.Rad2Deg;
        float newRot = Mathf.Atan2(differenceBetweenLocations.x, differenceBetweenLocations.y) * Mathf.Rad2Deg;//This does some weird maths and spits out an angle between -180 and 180. Its the guards desired Z rotation
        myTransform.rotation = Quaternion.Euler(0f, 0f, newRot);

        PrepareForNextPatrol();
    }

    void PrepareForNextPatrol()
    {
        //Prepare for the next patrol by setting the patrol number, and setting the target location and rotation. 

        //When the guard reaches the end of the patrol route, they'll start heading back up the patrol route. Which is what these if statements do. A switch might have been better here. ill have to remember to overuse them in my next project becus theyre my new scrunkly
        if (currentPatrol == patrolLocations.Length - 1)
            headingUpPatrolRoute = false;
        else if (currentPatrol == 0)
            headingUpPatrolRoute = true;

        if (headingUpPatrolRoute)
            currentPatrol += 1;
        else
            currentPatrol -= 1;

        //each patrol location is a vector3 within world space. 
        nextLocation = patrolLocations[currentPatrol];

        //found this code online, that should work as a LookAt function for 2D
        Vector3 differenceBetweenLocations = nextLocation - myTransform.position;
        rotationGoal = Mathf.Atan2(differenceBetweenLocations.y, differenceBetweenLocations.x) * Mathf.Rad2Deg;//This needs to be in euler angles. Euler angles work 0-360. This works -180 to 180 (i think.) So whenever the angle goal is negative, this wont work
        rotationGoal += 180f;//So I thought this would be a good solution (because if the angle is -180 in degree's. I thought itd be 0 in euler angles. but its actually 180 in euler angles

        currentBehaviour = Behaviours.patrolRotate;//The guard wants to rotate first, before starting to move to their next patrol location
        //Debug.Log("I am prepared for the next patrol at " + nextLocation + " at the rotation " + rotationGoal);
    }

    void Update()
    {
        //If guard is at their current patrol location, move onto the next location. or if we're at the end, start moving backwards. 
        switch (currentBehaviour)
        {
            case Behaviours.patrolRotate:
                RotationState();
                break;//from what i understand, a break is like return, but only for the switch. so if you didn't include break, you could have multiple cases at once. 
            case Behaviours.patrolWalk:
                WalkState();
                break;
            //case Behaviours.investigate:
                //break;
        }

    }

    void RotationState()
    {
        float rotationSpeed = 45f;//whilst these values (and the similar one in WalkState() could be editable through the inspector, I want all the guards to move and rotate at the same speed
        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, Quaternion.Euler(0f, 0f, rotationGoal), rotationSpeed * Time.deltaTime);
        if (myTransform.localEulerAngles.z >= rotationGoal - 3f && myTransform.localEulerAngles.z <= rotationGoal + 3f)//im getting from a range of values here because i wasnt confident in the values matching up exactly. Im not sure if this precaution is actually needed though
        {
            myTransform.rotation = Quaternion.Euler(0f, 0f, rotationGoal);//snap the guards rotation into the right spot (because we're close enough. but the rotation does need to be exact)
            currentBehaviour = Behaviours.patrolWalk;//change the behaviour now the guard has reached theyre rotation goal. Its time to walk!
        }
    }

    void WalkState()
    {
        //walk a speed value per second, towards the location goal

        float movementSpeed = 3f;

        float distance = Vector3.Distance(myTransform.position, nextLocation);//Calculate how far the guard is from theyre destination
        if (distance < movementSpeed * Time.deltaTime)
        {
            //This happens if the guard would reach their destination this frame. We snap the guard into position (so they dont overshoot)
            myTransform.position = nextLocation;
            PrepareForNextPatrol();//set up the next patrol (gets the guard to rotate again)
        }
        else//if the guard isnt close enough to their destination, they should just move forward at a constant pace
            myTransform.position += -myTransform.right * movementSpeed * Time.deltaTime;
       
    }

    void OnDrawGizmosSelected()
    {
        //This draws the guards patrol route. 
        Gizmos.color = Color.red;
        for (int x = 0; x < patrolLocations.Length; x++)
        {
            if (x != 0)
                Gizmos.DrawLine(patrolLocations[x - 1], patrolLocations[x]);
        }
    }
}



//code i removed just incase its needed later

/*      (from the set up next patrol function. This is how i was trying to set the next rotation goal. LookAt does not work in 2D. 
 *      //Vector3 currentRotation = myTransform.eulerAngles;
        //myTransform.LookAt(nextLocation, Vector3.forward);
        //emptyTransform.LookAt(nextLocation, Vector3.forward);
        //rotationGoal = myTransform.eulerAngles.z;
        //rotationGoal = emptyTransform.eulerAngles.z;
        //myTransform.eulerAngles = currentRotation;
*/

/*if (rotationGoal >= 0f)
            {
                float rotationZ = myTransform.eulerAngles.z + (Time.deltaTime * 45f);
                myTransform.eulerAngles = new Vector3(0f, 0f, rotationZ);
                currentRotationValue += 45f * Time.deltaTime;

                if (currentRotationValue >= rotationGoal)
                {
                    Debug.Log(currentRotationValue);
                    myTransform.eulerAngles = new Vector3(0f, 0f, rotationGoal);
                    rotating = false;
                }
            }
            else if (rotationGoal < 0f)
            {
                float rotationZ = myTransform.eulerAngles.z - (Time.deltaTime * 45f);
                myTransform.eulerAngles = new Vector3(0f, 0f, rotationZ);
                currentRotationValue -= 45f * Time.deltaTime;

                if (currentRotationValue <= rotationGoal)
                {
                    Debug.Log(currentRotationValue);
                    myTransform.eulerAngles = new Vector3(0f, 0f, rotationGoal);
                    rotating = false;
                }
            }*/

/*
if (rotationGoal > myTransform.eulerAngles.z)
{
    float rotationZ = myTransform.eulerAngles.z + (Time.deltaTime * 45f);
    myTransform.eulerAngles = new Vector3(0f, 0f, rotationZ);
    if (myTransform.eulerAngles.z >= rotationGoal)
    {
        myTransform.eulerAngles = new Vector3(0f, 0f, rotationGoal);
        rotating = false;
    }

    //Debug.Log("my rotation is: " + myTransform.eulerAngles.z);
}
else if (rotationGoal < myTransform.eulerAngles.z)
{
    float rotationZ = myTransform.eulerAngles.z - (Time.deltaTime * 45f);
    myTransform.eulerAngles = new Vector3(0f, 0f, rotationZ);
    if (myTransform.eulerAngles.z <= rotationGoal)
    {
        myTransform.eulerAngles = new Vector3(0f, 0f, rotationGoal);
        rotating = false;
    }

    //Debug.Log("my rotation is: " + myTransform.eulerAngles.z);
}
else if (rotationGoal == myTransform.eulerAngles.z)
{
    myTransform.eulerAngles = new Vector3(0f, 0f, rotationGoal);
    rotating = false;
}*/

/* (this was inside the update function for testing purposes)
if (Input.GetKeyDown(KeyCode.Space))
{
    Debug.Log("my rotation is: " + myTransform.eulerAngles.z);
}*/