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
        float angle = (Mathf.Atan2(mousePosition.y - rb.position.y, mousePosition.x - rb.position.x) * Mathf.Rad2Deg);

        if (Input.GetMouseButtonDown(0) && bulletsInChamber > 0)
        {
            shootBullet(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            Debug.Log("Shoot");
            bulletsInChamber--;
            ammoText.text = bulletsInChamber.ToString() + "/6";
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

    IEnumerator reload()
    {
        ammoText.text = ".../6";
        ammoText.transform.position += new Vector3 (-11.5f, 0, 0);
        yield return new WaitForSeconds(1.2f);
        bulletsInChamber = 6;
        ammoText.transform.position += new Vector3 (11.5f, 0, 0);
        ammoText.text = bulletsInChamber.ToString() + "/6";
        isReloading = false;
    }
}
