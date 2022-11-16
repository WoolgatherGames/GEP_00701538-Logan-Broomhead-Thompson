using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HM_HackableObject))]
public class O_SecurityCamera : MonoBehaviour, IInteractables
{
    Transform myTransform;
    HM_HackableObject myHackable;

    /*[Tooltip("This should be the game object with the SM_DetectionField script attached")]
    [SerializeField] GameObject detectionGameObject;
    [Tooltip("This is the game object that visually represents the detection fields field of view")]
    [SerializeField] GameObject visualiser;*/

    [Tooltip("A parent gameobject that contains all the objects attatched to the camera that should toggle on/off when the camera is turned on/off. The SM_detectionField and visualiser should be children of this object")]
    [SerializeField] GameObject toggleableObjectsParent;

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
    
    bool swingingForward;
    bool stopSwinging;

    bool securityCameraOn;

    private void Start()
    {
        securityCameraOn = true;
        rotationZ = fromRotationZ;
        restTimer = 0f;
        swingingForward = true;
        stopSwinging = false;
        myTransform = this.gameObject.GetComponent<Transform>();
        myHackable = this.gameObject.GetComponent<HM_HackableObject>();
    }

    private void Update()
    {
        if (!securityCameraOn)
            return;

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

    public void Interact()
    {
        //Debug.Log("ive been interacted with. whoo");

        if (myHackable.HackInteract())
        {
            //If HackInteract returns true, it means the object has already been hacked, so we can perform this objects functionality

            ToggleCameraOnOff();
        }
        else
            return;//HackInteract returns false if the object hasnt been hacked yet

    }


    void ToggleCameraOnOff()
    {
        securityCameraOn = !securityCameraOn;

        if (securityCameraOn)
        {
            toggleableObjectsParent.SetActive(true);
        }
        else
        {
            toggleableObjectsParent.SetActive(false);
        }
    }

}
