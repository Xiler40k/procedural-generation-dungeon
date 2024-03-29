using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wizardScript : MonoBehaviour
{
    public GameObject mark;
    public Vector2 wizardVector = new Vector2(3, 1);
    public GameObject npcGUI;

    public string[] bulletColors2 = new string[] {"White", "Orange", "Blue", "Green", "Yellow"};

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseVector = new Vector2(mousePosition.x, mousePosition.y) - wizardVector;

        if (mouseVector.magnitude < 2) {
            if (Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt("Interact"))) {
                mark.SetActive(false);
                npcGUI.SetActive(true);
                PlayerPrefs.SetInt("Paused", 1);
            }
        }
    }
}
