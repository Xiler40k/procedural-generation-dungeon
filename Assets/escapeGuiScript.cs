using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class escapeGuiScript : MonoBehaviour
{
    public GameObject settingsMenu;
    
    public void exitGame() {
        SceneManager.LoadScene("Main Menu");
    }

    public void loadSettingsPage() {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
