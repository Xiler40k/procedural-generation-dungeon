using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockwaveScript : MonoBehaviour
{
    public bool shockwaveEnabled = true;
    private float expansionTime = 0.5f;
    private float targetSize = 7;
    void Update()
    {
        //expand scale from 0 to 7 over 2 seconds
        if (this.transform.localScale.x < targetSize)
        {
            //expand over 2s
        } else 
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Character" && shockwaveEnabled == true)
        {
            var combatScript = GameObject.Find("gun").GetComponent<combat>();
            combatScript.health -= 1;
            shockwaveEnabled = false;
        }
    }
}
