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
        myTransform.LookAt(nextLocation, Vector3.forward);
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

        nextLocation = patrolLocations[currentPatrol];
        Vector3 currentRotation = myTransform.eulerAngles;
        myTransform.LookAt(nextLocation, Vector3.forward);
        rotationGoal = myTransform.eulerAngles.z;
        myTransform.eulerAngles = currentRotation;

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
        if (rotating)
        {
            float rotationZ = myTransform.eulerAngles.z + (Time.deltaTime * 10f);
            myTransform.eulerAngles = new Vector3(0f, 0f, rotationZ);
            if (myTransform.eulerAngles.z > rotationGoal)
            {
                myTransform.eulerAngles = new Vector3(0f, 0f, rotationGoal);
                rotating = false;
            }
        }
        else
        {
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