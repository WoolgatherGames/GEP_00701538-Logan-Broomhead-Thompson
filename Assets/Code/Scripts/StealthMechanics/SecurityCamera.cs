using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    Transform myTransform;
    float lerpValue;
    [Tooltip("How far (in degrees) will the camera swing per second. default is 30")]
    [SerializeField] float swingingSpeed = 30f;

    [Tooltip("How long does the camera wait before turning")]
    [SerializeField] float _restTime = 0.5f;
    float restTimer;

    [Tooltip("the rotation that the camera starts at and return too. Make sure this is lower than the to rotation")]
    [SerializeField] float fromRotationZ;
    [SerializeField] float toRotationZ;
    float rotationZ;


    //[Tooltip("The camera rotate on the Z axis from sweep angle x to sweep angle y. Make sure angle Y is larger than angle X")]
    
    bool swingingForward;
    bool stopSwinging;

    private void Start()
    {
        rotationZ = fromRotationZ;
        restTimer = 0f;
        swingingForward = true;
        stopSwinging = false;
        myTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        myTransform.eulerAngles = new Vector3(0f, 0f, rotationZ);

        if (!stopSwinging)
        {
            if (swingingForward)
            {
                rotationZ += Time.deltaTime * swingingSpeed;
                if (rotationZ > toRotationZ)
                {
                    stopSwinging = true;
                }
            }
            else
            {
                rotationZ -= Time.deltaTime * swingingSpeed;
                if (rotationZ < fromRotationZ)
                {
                    stopSwinging = true;
                }
            }
        }
        else
        {
            restTimer += Time.deltaTime;
            if (restTimer > _restTime)
            {
                restTimer = 0f;
                swingingForward = !swingingForward;
                stopSwinging = false;
            }
        }
    }

}
