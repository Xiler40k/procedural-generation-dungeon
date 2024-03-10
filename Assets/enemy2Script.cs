using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//chasing and melee bot
public class enemy2Script : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    public float speed = 4f;
    public float health = 5f;
    private float targetDistance = 20f;
    int damage = 1;
    int stunAmount = 10;
    public bool isChasing = false;
    public bool isAttacking = false;
    public bool canAttack = true;
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
            Destroy(gameObject);
        }

        if (shouldBeChasing() && !isStunned)
        {
            isChasing = true;
            direction = rbPlayer.position - rb.position;
            rb.velocity = direction.normalized * speed;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            rb.rotation = angle;
        } else {
            isChasing = false;
        }

        if(rb.velocity.magnitude <= 0.1 && isChasing == true && isAttacking == false)
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Character" && canAttack == true)
        {
            //Deals damage to player
            GameObject.Find("gun").GetComponent<combat>().health -= damage;
            Debug.Log("H: " + GameObject.Find("gun").GetComponent<combat>().health);
            canAttack = false;
            //deal knockback to enemy
            isStunned = true;
            //moves enemy away from player and delays movement and next attack
            StartCoroutine(stunDelay());
            Vector2 knockbackDirection = (rb.position - rbPlayer.position).normalized;
            rb.AddForce(knockbackDirection * stunAmount, ForceMode2D.Impulse);
            StartCoroutine(attackDelay());
        }
    }

    IEnumerator stunDelay()
    {
        yield return new WaitForSeconds(0.6f);
        isStunned = false;
    }

    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(2);
        canAttack = true;
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
