using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletScript : MonoBehaviour
{
    public int damage = 1;
    public bool isPaused = false;

    void Update()
    {
        if (PlayerPrefs.GetInt("Paused") == 1) {
            isPaused = true;
        } else {
            isPaused = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.tag == "Character")
        {
            if (!isPaused) {
                GameObject.Find("gun").GetComponent<combat>().takeDamage(damage);
            }
            Debug.Log("H: " + GameObject.Find("gun").GetComponent<combat>().playerHealth);
        }
    }
}
