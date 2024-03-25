using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    public Rigidbody2D rbEnemyBullet;
    public float speed = 1f;
    public float[] healthArray = {50f, 100f, 150f};
    public float health = 50f;
    public float bulletVelocity = 6f;
    public bool isChasing = false;
    public bool isShooting = false;
    public bool canShoot = true;
    public bool isStunned = false;
    public Vector2 direction;
    public bool isBossStarted = false;
    private bool bossTimerWaited = true;



    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        rbPlayer = GameObject.Find("Character").GetComponent<Rigidbody2D>();

        var difficulty = PlayerPrefs.GetString("difficulty");
        if (difficulty == "medium") {
            health = healthArray[1];
        } else if (difficulty == "hard") {
            health = healthArray[2];
        }
    }

    void Update() {

        if (health <= 0) {
            Destroy(gameObject);
            //spawn chest
        }

        if (isBossStarted) {
            int randomAttack = -1;
            if (bossTimerWaited) {
                StartCoroutine(bossTimer());
                randomAttack = Random.Range(0, 1);
            } else {
                randomAttack = -1;
            }

            if (randomAttack == 0) {
                changeColor();
            }

        } else if (Input.GetKeyDown(KeyCode.B) && (new Vector2(rbPlayer.transform.position.x, rbPlayer.transform.position.y) - new Vector2(40,0)).magnitude < 10) {
            isBossStarted = true;
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
        yield return new WaitForSeconds(3.5f);
        bossTimerWaited = true;
    }
}
