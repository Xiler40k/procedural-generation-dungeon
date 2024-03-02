using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1Script : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3f;
    public int health = 3;
    private Rigidbody2D rbPlayer;
    public Rigidbody2D rbEnemyBullet;
    public bool isChasing = false;
    public bool isShooting = false;
    public bool canShoot = true;
    public Vector2 direction;

    public float bulletVelocity = 6f;

    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        rbPlayer = GameObject.Find("Character").GetComponent<Rigidbody2D>();
    }   

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }


        if (shouldBeChasing() && isShooting == false)
        {
            isChasing = true;
            direction = rbPlayer.position - rb.position;
            rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime);
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            rb.rotation = angle;
        } else {
            isChasing = false;
        }

        if (canShoot && getDistance() < 2.5)
        {
            StartCoroutine(attack());
            rb.velocity = new Vector2(0, 0);
            isChasing = false;

        }

        if(rb.velocity.magnitude <= 0.1 && isChasing == true && isShooting == false)
        {
            //try a suitable direction based on direction from character to player
            direction = rbPlayer.position - rb.position;
            rb.velocity = new Vector2((direction.x < 0 ? -3 : 3) * speed, (direction.y < 0 ? -3 : 3) * speed);
        }
    }

    bool shouldBeChasing()
    {
        var dist = getDistance();
        if (dist < 5)
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
        shoot(rbPlayer.position - rb.position);
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
        yield return new WaitForSeconds(2.0f);
        canShoot = true;
    }

    void shoot(Vector2 direction){
        for (int i = 0; i < 10; i++)
        {
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            angle += (i * 36);
            Vector2 newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            shootBullet(newDirection);
        }
    }

    void shootBullet(Vector2 direction)
    {
        Rigidbody2D enemyBullet = Instantiate(rbEnemyBullet, rb.position + (direction.normalized * 0.5f), Quaternion.identity);
        enemyBullet.velocity = direction.normalized * bulletVelocity;
    }
}