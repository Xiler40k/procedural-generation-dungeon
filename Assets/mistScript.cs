using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mistScript : MonoBehaviour
{
    public GameObject mist;
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Mist1");
        if (collision.gameObject.tag == "Character")
        {
            Debug.Log("Mist2");
            Destroy(mist);
        }
    }
}
