using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1Script : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    public Rigidbody2D rbEnemyBullet;
    public float speed = 3f;
    public float health = 3f;
    private float targetDistance = 10f;
    private float attackDistance = 5f;
    public float bulletVelocity = 6f;
    public bool isChasing = false;
    public bool isShooting = false;
    public bool canShoot = true;
    public bool isStunned = false;
    public Vector2 direction;

    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        rbPlayer = GameObject.Find("Character").GetComponent<Rigidbody2D>();
    }   

    void Update()
    {
        if (health <= 0)
        {
            /*
            positionOfDeath = rb.position;
            GameObject.Find("lootSystem").GetDrop();
            */
            Destroy(gameObject);
        }


        if (shouldBeChasing() && isShooting == false && isStunned == false)
        {
            isChasing = true;
            direction = rbPlayer.position - rb.position;
            rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime);
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            rb.rotation = angle;
        } else {
            isChasing = false;
        }

        if (canShoot && getDistance() < attackDistance)
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
        if (dist < targetDistance)
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