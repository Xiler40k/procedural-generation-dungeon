using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float playerSpeed;
    public Rigidbody2D rb;
    private Vector2 playerDirection;

    // Update is called once per frame
    void Update()
    {
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        playerDirection.y = Input.GetAxisRaw("Vertical");

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + playerDirection * playerSpeed * Time.fixedDeltaTime);
    }

}
