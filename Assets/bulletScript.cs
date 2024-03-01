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
    }
}
