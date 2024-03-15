using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class primarykeyScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            Debug.Log("Key Collected");
            var n = PlayerPrefs.GetInt("keysCollected");
            n++;
            PlayerPrefs.SetInt("keysCollected", n);
            GameObject.Find("keyFlag" + n).GetComponent<TilemapRenderer>().enabled = true;

            if (n == 3)
            {
                //open laser
                GameObject.Find("LaserGroup").SetActive(false);
                Debug.Log("Laser Opened");
            }
            Destroy(gameObject);
        }
    }
}
