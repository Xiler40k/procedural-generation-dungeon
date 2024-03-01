using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D rbGun;
    public Rigidbody2D rbBullet;
    public float bulletDistance = 0.74f;
    float bulletVelocity = 5f;

    public int bulletsInChamber = 6;

    public TMPro.TextMeshProUGUI ammoText;

    bool isReloading = false;

    
    // Update is called once per frame
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && bulletsInChamber > 0)
        {
            //passes vector from player to mouse
            shootBullet(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            Debug.Log("Shoot");
            //removes bullet from chamber
            bulletsInChamber--;
            //change text on screen
            ammoText.text = bulletsInChamber.ToString() + "/6";
        }
        //if space bar pressed 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bulletsInChamber == 6)
            {
                shootCircle(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            isReloading = true;
            StartCoroutine(reload());
        }
    }

    void shootBullet(Vector2 direction)
    {
        Rigidbody2D bullet = Instantiate(rbBullet, rb.position + (direction.normalized * bulletDistance), Quaternion.identity);
        bullet.velocity = direction.normalized * bulletVelocity;
    }

    void shootCircle(Vector2 direction)
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            angle += (i * 60);
            Vector2 newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            shootBullet(newDirection);
            bulletsInChamber = bulletsInChamber - 6;
        }
    }

    IEnumerator reload()
    {
        //reload text
        ammoText.text = ".../6";
        //move text to left so no visible change
        ammoText.transform.position += new Vector3 (-11.5f, 0, 0);
        //wait for 1.2 seconds
        yield return new WaitForSeconds(1.2f);
        //reset bullets in chamber
        bulletsInChamber = 6;
        //move text back to original position and change text to 6/6
        ammoText.transform.position += new Vector3 (11.5f, 0, 0);
        ammoText.text = bulletsInChamber.ToString() + "/6";
        isReloading = false;
    }
}
