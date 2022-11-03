using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_RaycastFromRenderTexture : MonoBehaviour
{
    //ignore this script. found an alternate solution

    //this should be attatched to the main camera, NOT the camera that prints to the render texture

    [SerializeField] Camera mainCam;
    [SerializeField] Camera renderTextureCam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool hit = Physics.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 100f);

        }
    }
}
