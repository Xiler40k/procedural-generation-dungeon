using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movement : MonoBehaviour
{
    public float playerSpeed;
    public Rigidbody2D rb;

    public Rigidbody2D rbGun;
    private Vector2 playerDirection;

    private Vector2 mousePosition;

    // Update is called once per frame
    void Update()
    {
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        playerDirection.y = Input.GetAxisRaw("Vertical");

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = (Mathf.Atan2(mousePosition.y - rb.position.y, mousePosition.x - rb.position.x) * Mathf.Rad2Deg) ; // might not need -90

        float gunDistance = 0.72f;
        Vector3 gunPosition3 = Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0);
        Vector2 gunPosition = rb.position + new Vector2(gunPosition3.x, gunPosition3.y);

        rbGun.transform.position = gunPosition;
        rbGun.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + playerDirection * playerSpeed * Time.fixedDeltaTime);
    }
}