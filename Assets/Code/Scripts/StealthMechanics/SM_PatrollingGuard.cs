using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_PatrollingGuard : MonoBehaviour
{
    public enum Behaviours
    {
        patrol,//the guards default state 
        investigate,//when the guard hears a sound and wants to investigate it
    };

    Behaviours currentBehaviour = Behaviours.patrol;

    [Tooltip("The world locations of where this guard will move between. They will move in order. The guard will walk in STRAIGHT LINES. make sure theres no walls in their route. Keep the Z axis clear")]
    [SerializeField] Vector3[] patrolLocations;

    //whilst true, once the guard reaches a patrol location. they will move to the nxt once. whilst false, they will move down the patrol locations instead. 
    bool headingUpPatrolRoute;
    //the current number of patrol location that the guard is heading towards
    int currentPatrol;
    //the location that the guard is heading towards
    Vector3 nextLocation;

    //is the guard currently rotating. Whilst rotating, they shouldn't move.  
    bool rotating;

    Transform myTransform;
    Rigidbody2D rb;
    float rotationGoal;

    //[SerializeField] Transform emptyTransform;//rotate this to the desired location and then rotate towards it. it should be sat EXACTLY ontop of the guard. 

    bool rotateRight;//If true, the guard will rotate rightways, if false the guard will rotate leftways (whilst in rotating mode only)

    void Start()
    {
        myTransform = this.gameObject.GetComponent<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        //Set the guards starting location
        myTransform.position = patrolLocations[0];

        //set first location goal, then point the guard towards it
        headingUpPatrolRoute = true;
        currentPatrol = 1;
        nextLocation = patrolLocations[currentPatrol];

        //myTransform.LookAt(nextLocation, Vector3.forward);
        //https://answers.unity.com/questions/1023987/lookat-only-on-z-axis.html found this online as a replacement to LookAt() for 2d. 
        Vector3 differenceBetweenLocations = nextLocation - myTransform.position;
        //rotationGoal = Mathf.Atan2(differenceBetweenLocations.y, differenceBetweenLocations.x) * Mathf.Rad2Deg;
        float newRot = Mathf.Atan2(differenceBetweenLocations.x, differenceBetweenLocations.y) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(0f, 0f, newRot);

        rotating = false;
    }

    void PrepareForNextPatrol()
    {
        if (currentPatrol == patrolLocations.Length - 1)
            headingUpPatrolRoute = false;
        else if (currentPatrol == 0)
            headingUpPatrolRoute = true;

        if (headingUpPatrolRoute)
            currentPatrol += 1;
        else
            currentPatrol -= 1;


        //note to self: LookAt doesnt seem to work in 2d
        nextLocation = patrolLocations[currentPatrol];

        //Vector3 currentRotation = myTransform.eulerAngles;
        //myTransform.LookAt(nextLocation, Vector3.forward);
        //emptyTransform.LookAt(nextLocation, Vector3.forward);
        //rotationGoal = myTransform.eulerAngles.z;
        //rotationGoal = emptyTransform.eulerAngles.z;
        //myTransform.eulerAngles = currentRotation;

        //found this code online, that should work as a LookAt function for 2D
        Vector3 differenceBetweenLocations = nextLocation - myTransform.position;
        rotationGoal = Mathf.Atan2(differenceBetweenLocations.x, differenceBetweenLocations.y) * Mathf.Rad2Deg;//This needs to be in euler angles. Euler angles work 0-360. This works -180 to 180 (i think.) So whenever the angle goal is negative, this wont work
        rotationGoal += 180f;//So I thought this would be a good solution (because if the angle is -180 in degree's. I thought itd be 0 in euler angles. but its actually 180 in euler angles

        /*if (rotationGoal > myTransform.eulerAngles.z)
            rotateRight = true;
        else
            rotateRight = false;*/

        rotating = true;
        Debug.Log("I am prepared for the next patrol at " + nextLocation + " at the rotation " + rotationGoal);
    }

    void Update()
    {
        //If guard is at their current patrol location, move onto the next location. or if we're at the end, start moving backwards. 
        /*switch (currentBehaviour)
        {
            case Behaviours.patrol:
                Patrolling();
                break;

            case Behaviours.investigate:


                break;
        }*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("my rotation is: " + myTransform.eulerAngles.z);
        }

        if (rotating)
        {
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
            }

        }
        else
        {
            //the following line is for testing ONLY
            myTransform.position = nextLocation;


            PrepareForNextPatrol();
            return;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int x = 0; x < patrolLocations.Length; x++)
        {
            if (x != 0)
                Gizmos.DrawLine(patrolLocations[x - 1], patrolLocations[x]);
        }
    }
}