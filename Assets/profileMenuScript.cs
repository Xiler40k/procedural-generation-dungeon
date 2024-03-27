using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class profileMenuScript : MonoBehaviour
{
    public TMP_Text mobsKilled;
    public TMP_Text easyCompletions;
    public TMP_Text mediumCompletions;
    public TMP_Text hardCompletions;
    public GameObject mainMenu;
    public GameObject profileMenu;
    public void Awake() {
        mobsKilled.text = PlayerPrefs.GetInt("enemiesKilledStat").ToString();
        easyCompletions.text = PlayerPrefs.GetInt("easyCompletions").ToString();
        mediumCompletions.text = PlayerPrefs.GetInt("mediumCompletions").ToString();
        hardCompletions.text = PlayerPrefs.GetInt("hardCompletions").ToString();
    }

    public void backButton()
    {
        mainMenu.SetActive(true);
        profileMenu.SetActive(false);
    }
}
