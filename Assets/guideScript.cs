using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class guideScript : MonoBehaviour
{
    private string[] dialogues = new string[] {
        "Hello, I am the resident wizard here. I will help you get to grips with the game",
        "You can move with the WASD keys, and aim with your mouse.",
        "You can shoot with the left mouse button, and reload with the R key.",
        "Using right click, you can cycle through different bullet colors. Hit enemies with the wrong color and you'll do 5x less damage!",
        "You can teleport once every 10s with 'T' - a timer can be seen in the top right corner.",
        "Collect a key from each path to unlock the boss room to your right.",
        "Press escape to toggle the pause menu, where you can access settings or exit the game!",
        "Good luck adventurer! Remeber to keep check of your health...."
    };

    private int dialoguesNumber = 0;
    public TextMeshProUGUI dialoguesBox;
    private bool dialoguesFinished = true;

    void Awake() {
        dialoguesBox.text = "";
        PlayerPrefs.SetInt("Paused", 1);
        nextdialogues();
    }
    void Update() {
        if (Input.GetMouseButtonDown(0) && dialoguesFinished) {
            StartCoroutine(nextdialogues());
        }
    }

    IEnumerator nextdialogues() {
        dialoguesBox.text = "";
        dialoguesFinished = false;

        if (dialoguesNumber >= dialogues.Length) {
            PlayerPrefs.SetInt("Paused", 0);
            Destroy(gameObject);
        }

        foreach (char c in dialogues[dialoguesNumber]) {
            dialoguesBox.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        dialoguesNumber++;
        dialoguesFinished = true;
    }
}
