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
    public TMPro.TextMeshProUGUI tpText;
    public bool canTp = true;
    bool isReloading = false;
    public int health = 6;

    
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Ded");
            //stop script
            this.enabled = false;
        }
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && bulletsInChamber > 0)
        {
            //passes vector from player to mouse
            shootBullet(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            //decreases bullets in chamber
            bulletsInChamber--;
            //change text on screen
            ammoText.text = bulletsInChamber.ToString() + "/6";
        }
        //if space bar pressed 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bulletsInChamber >= 6)
            {
                shootCircle(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            isReloading = true;
            StartCoroutine(reload());
        }

        if (Input.GetKeyDown(KeyCode.T) && canTp)
        {
            teleport(mousePosition);
        }
    }

    void shootBullet(Vector2 direction)
    {
        Rigidbody2D bullet = Instantiate(rbBullet, rb.position + (direction.normalized * bulletDistance), Quaternion.identity);
        bullet.velocity = direction.normalized * bulletVelocity;
    }

    void shootCircle(Vector2 direction) //player probs shouldnt have this?
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            angle += (i * 60);
            Vector2 newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            shootBullet(newDirection);
        }
        bulletsInChamber -= 6;
        ammoText.text = bulletsInChamber.ToString() + "/6";
    }

    void teleport(Vector3 mousePosition)
    {
        Vector2 mousePosition2 = new Vector2(mousePosition.x, mousePosition.y);
        if (mousePosition2.magnitude < 5)
        {
            rb.position = mousePosition;
            StartCoroutine(teleportTimer());
        }
    }

    IEnumerator teleportTimer()
    {
        var timerLeft = 10;
        canTp = false;
        tpText.text = "0:10";
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            timerLeft--;
            tpText.text = "0:0" + timerLeft.ToString();
        };
        canTp = true;
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