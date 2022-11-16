using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PC_CameraController : MonoBehaviour
{
    //attatch this to teh players camera in the stealth gameplay scene. 
    //camera follows the player but offsets slightly based on the mouse position on the screen. 

    Transform myTransform;
    Camera thisCamera;
    [SerializeField] Transform playerBody;

    [Tooltip("How far the camera can move left/right or up/down based on the mouse position (if the camera is at the edge of the screen, itll look the furthest")]
    [SerializeField] Vector2 cameraMaximumOffset;

    [SerializeField] Vector2 screenBoundsX;
    [SerializeField] Vector2 screenBoundsY;
    //[Tooltip("How far to the left of the screen in percentage should the mouse have to be before the game adjusts the camera angle to nothing"), Range(0,100)]
    //[SerializeField] float mousePosToCenterScreen = 50f;

    private void Start()
    {
        myTransform = this.transform;
        thisCamera = this.transform.GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.x = Mathf.Clamp(mousePos.x, screenBoundsX.x, screenBoundsX.y);
        mousePos.y = Mathf.Clamp(mousePos.y, screenBoundsY.x, screenBoundsY.y);

        float screenWidth = (screenBoundsX.y - screenBoundsX.x);
        float screenHeight = (screenBoundsY.y - screenBoundsY.x);

        float xOffsetPercentage = (mousePos.x - screenBoundsX.x) / screenWidth;
        float yOffsetPercentage = (mousePos.y - screenBoundsY.x) / screenHeight;

        float xOffset = Mathf.Lerp(-cameraMaximumOffset.x, cameraMaximumOffset.x, xOffsetPercentage);
        float yOffset = Mathf.Lerp(-cameraMaximumOffset.y, cameraMaximumOffset.y, yOffsetPercentage);

        /*if ((Input.mousePosition.x / Screen.width) < (mousePosToCenterScreen / 100))
        {
            //if the players camera is far enough to the left of the screen, the camera shouldnt be offset at all. 
            xOffset = 0f;
            yOffset = 0f;
        }*/

        Vector3 newPos = new Vector3(playerBody.transform.position.x + xOffset, playerBody.transform.position.y + yOffset, -10f);
        myTransform.position = newPos;

        if (Input.GetMouseButtonDown(0))
        {
            if (xOffsetPercentage != 0f && xOffsetPercentage != 1f && yOffsetPercentage != 0f && yOffsetPercentage != 1f)
            {
                //check if the players mouse is within the field of the render texture
                HandleClicks(xOffsetPercentage, yOffsetPercentage);
            }
        }
        
    }

    void HandleClicks(float xPos, float yPos)
    {
        //when the player clicks, theyre technically clicking a render texture, so that needs to be converted. 
        //xPos and yPos are a percentage of how far the click was across the screen 

        Vector3 clickLocation = thisCamera.ScreenToWorldPoint(new Vector3(xPos * thisCamera.pixelWidth, yPos * thisCamera.pixelHeight, 10f));
        //Debug.Log(Physics2D.OverlapCircle(clickLocation, 0.1f));

        foreach(Collider2D obj in Physics2D.OverlapCircleAll(clickLocation, 0.1f))
        {
            if (obj.gameObject.GetComponent<IInteractables>() != null)
            {
                obj.gameObject.GetComponent<IInteractables>().Interact();
            }
        }

        //oaef so i want to use this to call the hackable object script
        
        /*
        if (Physics2D.OverlapCircle(clickLocation, 0.1f).GetComponent<HM_HackableObject>() != null)
        {
            Physics2D.OverlapCircle(clickLocation, 0.1f).GetComponent<HM_HackableObject>().HackInteract();
        }*/
    }
}
