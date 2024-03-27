using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class winLossGUIScript : MonoBehaviour
{
    public GameObject winText;
    public GameObject lossText;
    public TMP_Text difficultyText;
    public TMP_Text seedText;
    void Awake() {
        if (PlayerPrefs.GetInt("Win") == 1) {
            winText.SetActive(true);
            lossText.SetActive(false);
        } else {
            lossText.SetActive(true);
            winText.SetActive(false);
        }

        difficultyText.text = PlayerPrefs.GetString("difficulty").ToUpper();
        seedText.text = PlayerPrefs.GetInt("seed").ToString();
    }

    public void mainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
