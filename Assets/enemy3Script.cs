using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sniper bot
public class enemy3Script : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    public Rigidbody2D rbEnemyBullet;
    public float speed = 3f;
    public float health = 2f;
    private float targetDistance = 9.5f;
    public float bulletVelocity = 60f; //sniper predicts 0.8 seconds ahead so 10/0.8 = 12.5 (as a base value)
    public bool isChasing = false;
    public bool isShooting = false;
    public bool canShoot = true;
    public bool isStunned = false;
    public bool isEscaping = false; //moves enemy away from player
    public Vector2 direction;
    //public int damage = 2
    public bool playerExitedSpawn = false;
    public bool isPaused = false;

    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        rbPlayer = GameObject.Find("Character").GetComponent<Rigidbody2D>();
    }   

    void Update()
    {
        if (PlayerPrefs.GetInt("exitedSpawn") == 1)
        {
            playerExitedSpawn = true;
        }

        if (PlayerPrefs.GetInt("Paused") == 1) {
            isPaused = true;
            rb.velocity = new Vector2(0,0);
        } else {
            isPaused = false;
        }

        if (health <= 0)
        {
            /*
            positionOfDeath = rb.position;
            GameObject.Find("lootSystem").GetDrop();
            */
            Destroy(gameObject);
        }

        if (getDistance() < 5)
        {
            isEscaping = true;
            direction = rb.position - rbPlayer.position;
            rb.velocity = direction.normalized * speed;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            rb.rotation = angle;
        } else {
            isEscaping = false;
        }

        if (shouldBeChasing() && isEscaping == false && isShooting == false && isStunned == false && playerExitedSpawn && !isPaused)
        {
            isChasing = true;
            direction = rbPlayer.position - rb.position;
            rb.velocity = direction.normalized * speed;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            rb.rotation = angle;
        } else {
            isChasing = false;
        }

        if (canShoot && getDistance() <= 10 && playerExitedSpawn && !isPaused)
        {
            StartCoroutine(attack());
            rb.velocity = new Vector2(0, 0);
            isChasing = false;
        }

        //Will need to improve next section for sniper

        if (rb.velocity.magnitude <= 0.1 && isChasing == true && isShooting == false && !isPaused)
        {
            //try a suitable direction based on direction from character to player
            direction = rbPlayer.position - rb.position;
            rb.velocity = new Vector2((direction.x < 0 ? -5 : 5) * speed, (direction.y < 0 ? -5 : 5) * speed);
        }
    }

    bool shouldBeChasing()
    {
        var dist = getDistance();
        // great than so it stops at larger distance
        if (dist > targetDistance && dist < 15)
        {
            return true;
        }
        return false;
    }

    float getDistance()
    {
        return Vector2.Distance(rb.position, rbPlayer.position);
    }

    IEnumerator attack()
    {
        isShooting = true;
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        //2 shots to combat player teleportation
        shootBullet();
        yield return new WaitForSeconds(1.5f);
        shootBullet();
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
        yield return new WaitForSeconds(1.3f);
        canShoot = true;
    }

    void shootBullet()
    {
        //take players diecrion velocity and calc where they will be in 0.8 seconds.
        var playerVelocity = rbPlayer.velocity * 0.8f;
        Vector2 futurePosition = rbPlayer.position + playerVelocity;
        //aim there and shoot
        direction = futurePosition - rb.position;
        bulletVelocity = 30f;
        
        Rigidbody2D enemyBullet = Instantiate(rbEnemyBullet, rb.position + (direction.normalized * 0.6f), Quaternion.identity);
        enemyBullet.velocity = direction.normalized * bulletVelocity;
    }

    public IEnumerator takeKnockback()
    {
        isStunned = true;
        Vector2 knockbackDirection = (rb.position - rbPlayer.position).normalized;
        rb.AddForce(knockbackDirection * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isStunned = false;
    }
}