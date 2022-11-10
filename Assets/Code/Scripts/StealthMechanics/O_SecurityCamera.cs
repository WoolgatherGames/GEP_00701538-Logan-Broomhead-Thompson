using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HM_HackableObject))]
public class O_SecurityCamera : MonoBehaviour, IInteractables
{
    Transform myTransform;
    HM_HackableObject myHackable;
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

    private void Start()
    {
        rotationZ = fromRotationZ;
        restTimer = 0f;
        swingingForward = true;
        stopSwinging = false;
        myTransform = this.gameObject.GetComponent<Transform>();
        myHackable = this.gameObject.GetComponent<HM_HackableObject>();
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

    public void Interact()
    {
        Debug.Log("ive been interacted with. whoo");
        myHackable.HackInteract();
    }

}
