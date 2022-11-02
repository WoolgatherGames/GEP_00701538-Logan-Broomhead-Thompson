using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PC_PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    Rigidbody2D rb;

    private void Start()
    {
        rb = this.transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.AddForce(new Vector2(moveX, moveY).normalized * movementSpeed);
    }
}
