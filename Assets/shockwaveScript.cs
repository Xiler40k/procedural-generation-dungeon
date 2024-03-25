using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockwaveScript : MonoBehaviour
{
    public bool shockwaveEnabled = true;
    private float expansionTime = 0.05f;
    private float targetSize = 7;
    public bool isPaused = false;
    
    void Update()
    {
        if (PlayerPrefs.GetInt("Paused") == 1) {
            isPaused = true;
        } else {
            isPaused = false;
        }

        //expand scale from 0 to 7 over 2 seconds
        if (this.transform.localScale.x < targetSize && !isPaused)
        {
            //expand over 2s
            this.transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime / expansionTime;
        } else 
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Character" && shockwaveEnabled == true)
        {
            var combatScript = GameObject.Find("gun").GetComponent<combat>();
            if (!isPaused) {
                combatScript.takeDamage(1);
            }
            shockwaveEnabled = false;
        }
    }
}
