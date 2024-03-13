using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class generationMenuScript : MonoBehaviour
{
    public void generateDungeon()
    {
        SceneManager.LoadScene("Game");
    }
}
