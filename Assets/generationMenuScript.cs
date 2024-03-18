using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class generationMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject generationMenu;
    public GameObject easyButton;
    public GameObject mediumButton;
    public GameObject hardButton;
    public GameObject seedErrorText;
    public TMP_InputField inputField;

    public void OnEnable()
    {
        if (PlayerPrefs.GetString("difficulty") == "easy") {
            easyButtonPressed();
        } else if (PlayerPrefs.GetString("difficulty") == "medium") {
            mediumButtonPressed();
        } else if (PlayerPrefs.GetString("difficulty") == "hard") {
            hardButtonPressed();
        } else {
            //default difficulty if none were selected before.
            easyButtonPressed();
        }
    }
    public void generateDungeon()
    {
        if (inputField.text == "")
        {
            PlayerPrefs.SetInt("generateDungeon", 1);
            SceneManager.LoadScene("Game");
            PlayerPrefs.SetInt("seed", 0);
        }
        else
        {
            if (int.Parse(inputField.text) > 1000000000 || int.Parse(inputField.text) < 0)
            {
                StartCoroutine(showSeedError());
                return;
            }
            PlayerPrefs.SetInt("seed", int.Parse(inputField.text));
            PlayerPrefs.SetInt("generateDungeon", 1);
            SceneManager.LoadScene("Game");
        }
    }

    public void backButton()
    {
        mainMenu.SetActive(true);
        generationMenu.SetActive(false);
    }

    public void easyButtonPressed()
    {
        easyButton.GetComponent<Image>().color = new Color(48/225f, 243/225f, 12/225f, 1f);
        mediumButton.GetComponent<Image>().color = new Color(8/225f, 241/225f, 236/225f, 70/255f);
        hardButton.GetComponent<Image>().color = new Color(245/225f, 8/225f, 33/225f, 100/255f);
        PlayerPrefs.SetString("difficulty", "easy");
    }

    public void mediumButtonPressed()
    {
        easyButton.GetComponent<Image>().color = new Color(48/225f, 243/225f, 12/225f, 83/255f);
        mediumButton.GetComponent<Image>().color = new Color(8/225f, 241/225f, 236/225f, 1f);
        hardButton.GetComponent<Image>().color = new Color(245/225f, 8/225f, 33/225f, 100/255f);
        PlayerPrefs.SetString("difficulty", "medium");
    }

    public void hardButtonPressed()
    {
        easyButton.GetComponent<Image>().color = new Color(48/225f, 243/225f, 12/225f, 83/255f);
        mediumButton.GetComponent<Image>().color = new Color(8/225f, 241/225f, 236/225f, 70/255f);
        hardButton.GetComponent<Image>().color = new Color(245/225f, 8/225f, 33/225f, 1f);
        PlayerPrefs.SetString("difficulty", "hard");
    }

    //could be re-done by getting highlighted button, chnagin its alpha value to 1 and other to 125/255f.

    IEnumerator showSeedError()
    {
        seedErrorText.SetActive(true);
        yield return new WaitForSeconds(3);
        seedErrorText.SetActive(false);
    }
}
