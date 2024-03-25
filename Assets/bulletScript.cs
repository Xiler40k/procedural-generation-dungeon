using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float damage = 1f;
    public combat playerCombatScript;
    void Start()
    {
        playerCombatScript = GameObject.Find("gun").GetComponent<combat>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.tag == "Enemy")
        {
            var enemy1Script = collision.gameObject.GetComponent<enemy1Script>();
            var enemy2Script = collision.gameObject.GetComponent<enemy2Script>();
            var enemy3Script = collision.gameObject.GetComponent<enemy3Script>();
            var enemy4Script = collision.gameObject.GetComponent<enemy4Script>();
            var enemyNum = 0;
            if (enemy1Script != null)
            {
                if (playerCombatScript.selectedColor == "Orange")
                {
                    enemy1Script.health -= damage;
                } else
                {
                    enemy1Script.health -= 0.2f * damage;
                }
                enemyNum = 1;

            } else if (enemy2Script != null)
            {
                if (playerCombatScript.selectedColor == "Green")
                {
                    enemy2Script.health -= damage;
                } else
                {
                    enemy2Script.health -= 0.2f * damage;
                }
                enemyNum = 2;
            } else if (enemy3Script != null)
            {
                if (playerCombatScript.selectedColor == "Blue")
                {
                    enemy3Script.health -= damage;
                } else
                {
                    enemy3Script.health -= 0.2f * damage;
                }
            } else if (enemy4Script != null)
            {
                if (playerCombatScript.selectedColor == "Yellow")
                {
                    enemy4Script.health -= damage;
                } else
                {
                    enemy4Script.health -= 0.2f * damage;
                }
            }

            if (enemyNum == 1)
            {
                collision.gameObject.GetComponent<enemy1Script>().StartCoroutine("takeKnockback");
            } else if (enemyNum == 2)
            {
                collision.gameObject.GetComponent<enemy2Script>().StartCoroutine("takeKnockback");
            } else if (enemyNum == 3)
            {
                collision.gameObject.GetComponent<enemy3Script>().StartCoroutine("takeKnockback");
            } else if (enemyNum == 4)
            {
                collision.gameObject.GetComponent<enemy4Script>().StartCoroutine("takeKnockback");
            }
        }

        if (collision.gameObject.tag == "Boss")
        {
            var bossScript = collision.gameObject.GetComponent<bossScript>();
            if (playerCombatScript.selectedColor == bossScript.currentColor)
            {
                bossScript.health -= damage;
            } else
            {
                bossScript.health -= 0.2f * damage;
            }
        }
    }
}
