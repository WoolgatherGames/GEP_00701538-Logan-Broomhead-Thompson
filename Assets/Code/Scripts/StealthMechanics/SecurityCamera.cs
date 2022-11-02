using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    //[SerializeField] float scanDistance;
    //[SerializeField] float coneDiameter;
    //[Tooltip("set this to player")]
    //[SerializeField] LayerMask searchingFor;
    Transform myTransform;
    [SerializeField] Quaternion fromRotation;
    [SerializeField] Quaternion toRotation;
    float lerpValue;
    [Tooltip("If this is equal to 1, it'll take 1 second for the camera to swing")]
    [SerializeField] float swingingSpeed = 1f;

    [Tooltip("How long does the camera wait before turning")]
    [SerializeField] float _restTime = 0.5f;
    float restTimer;


    //[Tooltip("The camera rotate on the Z axis from sweep angle x to sweep angle y. Make sure angle Y is larger than angle X")]
    
    bool swingingForward;

    private void Start()
    {
        lerpValue = 0f;
        restTimer = 0f;
        swingingForward = true;
        myTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        myTransform.rotation = Quaternion.Lerp(toRotation, fromRotation, lerpValue);

        if (swingingForward)
        {
            lerpValue += Time.deltaTime * swingingSpeed;
            if (lerpValue > 1f)
            {
                restTimer += Time.deltaTime;
                if (restTimer > _restTime)
                {
                    restTimer = 0f;
                    swingingForward = false;
                }
            }
        }
        else
        {
            lerpValue -= Time.deltaTime * swingingSpeed;
            if (lerpValue < 0f)
            {
                restTimer += Time.deltaTime;
                if (restTimer > _restTime)
                {
                    restTimer = 0f;
                    swingingForward = true;
                }
            }
        }
    }

}
