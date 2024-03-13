using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserScript : MonoBehaviour
{
    public GameObject laser;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit1");
        if (collision.gameObject.tag == "Character")
        {
            Debug.Log("hit2");
            var combatScript = GameObject.Find("gun").GetComponent<combat>();
            combatScript.health -= 1;
            combatScript.StartCoroutine(combatScript.takeKnockback(laser.transform.position));
        }
    }
}
