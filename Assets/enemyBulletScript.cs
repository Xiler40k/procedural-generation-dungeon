using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletScript : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.tag == "Character")
        {
            GameObject.Find("gun").GetComponent<combat>().takeDamage(damage);
            Debug.Log("H: " + GameObject.Find("gun").GetComponent<combat>().playerHealth);
        }
    }
}
