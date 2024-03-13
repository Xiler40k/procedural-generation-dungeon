using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject generationMenu;
    public void play()
    {
        //ran into issues using gameObject.Find not finidng object
        mainMenu.SetActive(false);
        generationMenu.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }
}
