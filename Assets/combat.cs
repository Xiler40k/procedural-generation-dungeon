using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class combat : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D rbGun;
    public Rigidbody2D rbBullet;
    public GameObject panel;
    public GameObject bulletGameObject;
    public float bulletDistance = 0.74f;
    float bulletVelocity = 5f;
    public int bulletsInChamber = 6;
    public TMPro.TextMeshProUGUI ammoText;
    public TMPro.TextMeshProUGUI tpText;
    public bool canTp = true;
    public bool canMove = true;
    bool isReloading = false;
    public float playerHealth = 6;
    public string selectedColor = "White";
    public movement movementScript;
    public GameObject[] hearts;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;
    public AudioClip gunshot;
    public AudioSource audioSource;
    public GameObject pauseMenu;
    public int pauseInt = 0;

    
    // Update is called once per frame

    void Start()
    {
        bulletGameObject.GetComponent<SpriteRenderer>().color = bulletColorCodes2[0];
        panel.GetComponent<Image>().color = bulletColorCodes2[colorIndex];

        movementScript = GameObject.Find("Character").GetComponent<movement>();
        PlayerPrefs.SetInt("exitedSpawn", 0);
        PlayerPrefs.SetInt("Paused", 0);
        PlayerPrefs.SetInt("InSettingsMenu", 0);
        PlayerPrefs.SetInt("Win", 0);
        
        //PlayerPrefs.SetInt("Teleport", (int)KeyCode.T);
        //for testing purposes
    }
    void Update()
    {
        exitSpawnCheck();

        if (PlayerPrefs.GetInt("Paused") == 1) {
            canMove = false;
        } else {
            canMove = true;
        }

        if(!canMove) {
            rb.velocity = new Vector2(0,0);
        }

        if (playerHealth <= 0)
        {
            Debug.Log("Ded");
            //stop script
            this.enabled = false;
            PlayerPrefs.SetInt("Pasued", 1);

            //load tombstone and place it at player's closest tile position 
            StartCoroutine(lostGame());
        }
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Escape) && PlayerPrefs.GetInt("InSettingsMenu") == 0) {
            pauseInt = (pauseInt + 1) % 2;
            PlayerPrefs.SetInt("Paused", pauseInt);
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseInt == 1) {
                canMove = false;
            } else {
                canMove = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && bulletsInChamber > 0 && canMove)
        {
            //passes vector from player to mouse
            shootBullet(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            //decreases bullets in chamber
            bulletsInChamber--;
            //change text on screen
            ammoText.text = bulletsInChamber.ToString() + "/6";
        }
        //if space bar pressed 
        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            if (bulletsInChamber >= 6)
            {
                shootCircle(new Vector2 (mousePosition.x, mousePosition.y) - rb.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && canMove)
        {
            isReloading = true;
            StartCoroutine(reload());
        }

        if (Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt("Teleport")) && canTp && canMove)
        {
            teleport(mousePosition);
        }

        if (Input.GetMouseButtonDown(1) && canMove)
        {
            changeColor();
        } 
    }

    void exitSpawnCheck() {
        if (Mathf.Abs(rb.transform.position.x) > 17 || Mathf.Abs(rb.transform.position.y) > 10)
        {
            PlayerPrefs.SetInt("exitedSpawn", 1);
        }
        else {
            PlayerPrefs.SetInt("exitedSpawn", 0);
        }
    }

    public string[] bulletColors2 = new string[] {"White", "Orange", "Blue", "Green", "Yellow"};
    private int colorIndex = 0;
    public Color[] bulletColorCodes2 = new Color[] {new Color(243/255f, 235/255f, 235/255f, 255/255f), new Color(245/255f, 132/255f, 17/255f, 255/255f), new Color(21/255f, 217/255f, 233/255f, 255/255f), new Color(13/255f, 192/255f, 0/255f, 255/255f), new Color(255/255f, 247/255f, 1/255f, 255/255f)};


    void changeColor() {
        colorIndex = (colorIndex + 1) % 5;
        Debug.Log(colorIndex);
        selectedColor = bulletColors2[colorIndex];
        Debug.Log(selectedColor);
        bulletGameObject.GetComponent<SpriteRenderer>().color = bulletColorCodes2[colorIndex];
        //change GUI panel color
        panel.GetComponent<Image>().color = bulletColorCodes2[colorIndex];
    }

    void shootBullet(Vector2 direction)
    {
        Rigidbody2D bullet = Instantiate(rbBullet, rb.position + (direction.normalized * bulletDistance), Quaternion.identity);
        bullet.velocity = direction.normalized * bulletVelocity;
        //plays gunshot sound
        audioSource.PlayOneShot(gunshot);
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

    public IEnumerator takeKnockback(Vector2 objectPosition)
    {
        movementScript.canMove = false;
        Vector2 knockbackDirection = (rb.position - objectPosition).normalized;
        rb.AddForce(knockbackDirection * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        movementScript.canMove = true;
    }

    public void takeDamage(float damage)
    {
        playerHealth -= damage;
        var fullHearts = Math.Floor(Math.Ceiling(playerHealth) / 2) ;
        var halfHearts = playerHealth % 2;

        for (int i = 0; i < 3; i++)
        {
            if (i < fullHearts)
            {
                hearts[i].GetComponent<Image>().sprite = fullHeartSprite;
            }
            else if (i == fullHearts && halfHearts == 1)
            {
                hearts[i].GetComponent<Image>().sprite = halfHeartSprite;
            }
            else
            {
                hearts[i].GetComponent<Image>().sprite = emptyHeartSprite;
            }
        }
    }

    IEnumerator lostGame()
    {
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("Paused", 1);
        GameObject.Find("Boss").GetComponent<bossScript>().lost(); 
    }
}