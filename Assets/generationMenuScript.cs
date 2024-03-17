using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class generationMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject generationMenu;
    public GameObject easyButton;
    public GameObject mediumButton;
    public GameObject hardButton;
    public void generateDungeon()
    {
        SceneManager.LoadScene("Game");
    }

    public void backButton()
    {
        mainMenu.SetActive(true);
        generationMenu.SetActive(false);
    }

    public void easyButtonPressed()
    {
        easyButton.GetComponent<Image>().color = new Color(48/225f, 243/225f, 12/225f, 1f);
        mediumButton.GetComponent<Image>().color = new Color(8/225f, 241/225f, 236/225f, 83/255f);
        hardButton.GetComponent<Image>().color = new Color(245/225f, 8/225f, 33/225f, 132/255f);
    }

    public void mediumButtonPressed()
    {
        easyButton.GetComponent<Image>().color = new Color(48/225f, 243/225f, 12/225f, 83/255f);
        mediumButton.GetComponent<Image>().color = new Color(8/225f, 241/225f, 236/225f, 1f);
        hardButton.GetComponent<Image>().color = new Color(245/225f, 8/225f, 33/225f, 132/255f);
    }

    public void hardButtonPressed()
    {
        easyButton.GetComponent<Image>().color = new Color(48/225f, 243/225f, 12/225f, 83/255f);
        mediumButton.GetComponent<Image>().color = new Color(8/225f, 241/225f, 236/225f, 83/255f);
        hardButton.GetComponent<Image>().color = new Color(245/225f, 8/225f, 33/225f, 1f);
    }

    //could be re-done by getting highlighted button, chnagin its alpha value to 1 and other to 125/255f.
}
