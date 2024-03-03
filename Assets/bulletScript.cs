using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public int damage = 1;
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.tag == "Enemy")
        {
            var enemy1Script = collision.gameObject.GetComponent<enemy1Script>();
            var enemy2Script = collision.gameObject.GetComponent<enemy2Script>();
            //var enemy3Script = collision.gameObject.GetComponent<enemy3Script>();
            //var enemy4Script = collision.gameObject.GetComponent<enemy4Script>();
            if (enemy1Script != null)
            {
                enemy1Script.health -= damage;
            } else if (enemy2Script != null)
            {
                enemy2Script.health -= damage;
            } /* else if (enemy3Script != null)
            {
                enemy3Script.health -= damage;
            } else if (enemy4Script != null)
            {
                enemy4Script.health -= damage;
            } */
        }
    }
}
