using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBigScript : MonoBehaviour
{
    public GameObject laser;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit1");
        if (collision.gameObject.tag == "Character")
        {
            Debug.Log("hit2");
            var combatScript = GameObject.Find("gun").GetComponent<combat>();
            combatScript.takeDamage(1f);
            combatScript.StartCoroutine(combatScript.takeKnockback(laser.transform.position));
        }
    }
}
