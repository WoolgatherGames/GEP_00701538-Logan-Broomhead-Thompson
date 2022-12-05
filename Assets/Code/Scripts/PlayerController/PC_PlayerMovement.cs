using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PC_PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;//getting the perfect movement speed is difficult, and also dependant on how fast the cameras rotate + guards move. Ideally the player wants to move at a brisk pace so the game doesnt feel sluggish. They want to move at a similar speed to guards but not so fast that you can dodge them easily. The cameras rotation should be difficult to sneak by but not impossible. currently the ideal speed seems to be around 110
    Rigidbody2D rb;

    bool allowMovement;

    private void Start()
    {
        rb = this.transform.GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()//have to use fixed update. otherwise the inspector + build version are different
    {
        if (allowMovement)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            rb.AddForce(new Vector2(moveX, moveY).normalized * movementSpeed);
        }
    }

    public void EnableMovement()
    {
        allowMovement = true;
    }
    public void DisableMovement()
    {
        Debug.Log("I cant move");
        allowMovement = false;
    }
}
