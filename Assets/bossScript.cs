using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class bossScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    public Rigidbody2D rbLaser;
    public Rigidbody2D rbBigLaser;
    public Rigidbody2D rbEnemyBullet;
    private float speed = 2f;
    public float[] healthArray = {50f, 100f, 150f};
    public float health = 50f;
    private float maxHealth = 50f;
    private float bulletVelocity = 9f;
    public bool isChasing = false;
    public bool isShooting = false;
    public bool canShoot = true;
    public bool isStunned = false;
    public Vector2 direction;
    public bool isBossStarted = false;
    private bool bossTimerWaited = true;
    public GameObject WinLossGUI;
    public TMP_Text bossText;
    public bool hasWonGame = false;
    public bool isPaused = false;



    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        rbPlayer = GameObject.Find("Character").GetComponent<Rigidbody2D>();

        var difficulty = PlayerPrefs.GetString("difficulty");
        if (difficulty == "medium") {
            health = healthArray[1];
            maxHealth = healthArray[1];
        } else if (difficulty == "hard") {
            health = healthArray[2];
            maxHealth = healthArray[2];
        }

    }

    void Update() {

        if (health <= 0 && !hasWonGame) {
            PlayerPrefs.SetInt(PlayerPrefs.GetString("difficulty") + "Completions", PlayerPrefs.GetInt(PlayerPrefs.GetString("difficulty") + "Completions") + 1);

            //get coins and spawn where boss died.
            hasWonGame = true;
            rb.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(wonGame());
        }

        if (PlayerPrefs.GetInt("Paused") == 1) {
            isPaused = true;
            rb.velocity = new Vector2(0,0);
        } else {
            isPaused = false;
        }

        if (isBossStarted) {
            int randomAttack = -1;
            if (bossTimerWaited) {
                StartCoroutine(bossTimer());
                changeColor();
                if (health >= maxHealth/2) {
                    randomAttack = Random.Range(0, 3);
                } else {
                    var bigAttackRoll = Random.Range(0, 3);
                    if (bigAttackRoll <= 1) {
                        randomAttack = 3;
                    } else {
                        randomAttack = Random.Range(0, 3);
                    }
                randomAttack = Random.Range(3, 4);
            } 
            }
            else {
                randomAttack = -1;
                //move towards player
                direction = rbPlayer.position - rb.position;
                rb.velocity = direction.normalized * speed;
            }

            if (randomAttack == 0) {
                StartCoroutine(attack1());
            } else if (randomAttack == 1) {
                StartCoroutine(attack2());
            } else if (randomAttack == 2) {
                attack3();
            } else if (randomAttack == 3) {
                attack4();
            }

        } else if (Input.GetKeyDown(KeyCode.B) && (new Vector2(rbPlayer.transform.position.x, rbPlayer.transform.position.y) - new Vector2(40,0)).magnitude < 10) {
            isBossStarted = true;
            bossText.gameObject.SetActive(false);
            StartCoroutine(bossTimer());
            Debug.Log("Boss started");
        }
    }

    private string[] Colors2 = new string[] {"White", "Orange", "Blue", "Green", "Yellow"};
    private int colorIndex = 1;
    public Color[] ColorCodes2 = new Color[] {new Color(243/255f, 235/255f, 235/255f, 255/255f), new Color(245/255f, 132/255f, 17/255f, 255/255f), new Color(21/255f, 217/255f, 233/255f, 255/255f), new Color(13/255f, 192/255f, 0/255f, 255/255f), new Color(255/255f, 247/255f, 1/255f, 255/255f)};
    public string currentColor = "Orange";


    void changeColor() {
        var randomIncrease = Random.Range(0, 4);
        colorIndex = (colorIndex + randomIncrease) % 5;
        currentColor = Colors2[colorIndex];
        Debug.Log(currentColor);
        gameObject.GetComponent<SpriteRenderer>().color = ColorCodes2[colorIndex];
    }
    IEnumerator bossTimer() {
        bossTimerWaited = false;
        yield return new WaitForSeconds(6f);
        bossTimerWaited = true;
    }

    IEnumerator wonGame() {
        PlayerPrefs.SetInt("Win", 1);
        yield return new WaitForSeconds(3f);
        WinLossGUI.SetActive(true);
    }

    IEnumerator attack1() {
        for (int i = 0; i < 30; i++)
        {
            float angle = (Mathf.Atan2(rbPlayer.position.y - rb.position.y, rbPlayer.position.x - rb.position.x) * Mathf.Rad2Deg) - 45; //buffer degrees
            angle += (i * 12);
            Vector2 newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            shootBullet(newDirection);
            angle += 180;
            newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            shootBullet(newDirection);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void shootBullet(Vector2 direction)
    {
        if (!isPaused) {
            Rigidbody2D enemyBullet = Instantiate(rbEnemyBullet, rb.position + (direction.normalized * 1.4f), Quaternion.identity);
            enemyBullet.velocity = direction.normalized * bulletVelocity;
        }
    }

    IEnumerator attack2() {
        for (int i = 0; i < 18; i++)
        {
            float angle = (Mathf.Atan2(rbPlayer.position.y - rb.position.y, rbPlayer.position.x - rb.position.x) * Mathf.Rad2Deg);
            int randomAngleOffset = Random.Range(-45, 45);
            angle += randomAngleOffset;
            Vector2 newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            shootBullet(newDirection);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void attack3() {
        //instatniate laser(1) object and rotate around boss

        var laser = Instantiate(rbBigLaser, rb.position, Quaternion.identity);
        //rotate laser 360 degrees over 4 seconds
        StartCoroutine(rotateLaser(laser));
    }

    IEnumerator rotateLaser(Rigidbody2D laser) {
        for (int i = 0; i < 360; i++)
        {
            laser.transform.position = rb.position;
            laser.transform.Rotate(0, 0, 1);
            yield return new WaitForSeconds(0.006f);
        }
        Destroy(laser.gameObject);
    }

    void attack4() {
        var angle = Mathf.Atan2(rbPlayer.position.y - rb.position.y, rbPlayer.position.x - rb.position.x) * Mathf.Rad2Deg;
        var laser = Instantiate(rbLaser, rb.position, Quaternion.Euler(0, 0, angle));
        StartCoroutine(rotateLaser2(laser, "left"));
        var laser2 = Instantiate(rbLaser, rb.position, Quaternion.Euler(0, 0, angle));
        StartCoroutine(rotateLaser2(laser2, "right"));
    }
    
    IEnumerator rotateLaser2(Rigidbody2D laser, string direction) {
        //rotate the laser 90 degrees towardss player
        //
        if (direction == "left") {
            for (int i = 0; i < 90; i++)
            {
                laser.transform.position = rb.position;
                laser.transform.Rotate(0, 0, 1);
                yield return new WaitForSeconds(0.006f);
            }
        } else {
            for (int i = 0; i < 90; i++)
            {
                laser.transform.position = rb.position;
                laser.transform.Rotate(0, 0, -1);
                yield return new WaitForSeconds(0.006f);
            }
        }
        Destroy(laser.gameObject);
    }

    public void lost()
    {
        PlayerPrefs.SetInt("Win", 0);
        WinLossGUI.SetActive(true);
    }

}
