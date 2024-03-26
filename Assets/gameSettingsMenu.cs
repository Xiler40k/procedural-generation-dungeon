using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;

public class gameSettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject escapeMenu;
    private bool isWaitingForInput = false;
    private string keybindToChange;
    public AudioMixer audioMixer;

    void Awake() {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        PlayerPrefs.SetInt("InSettingsMenu", 1);

        GameObject.Find("TeleportText").GetComponent<TMPro.TextMeshProUGUI>().text = ((KeyCode)PlayerPrefs.GetInt("Teleport")).ToString();
        GameObject.Find("InteractText").GetComponent<TMPro.TextMeshProUGUI>().text = ((KeyCode)PlayerPrefs.GetInt("Interact")).ToString();
    }
    

    public void changeVolume(float volume) {
        volume = volumeSlider.value;
        float volumeDecibels = volume == 0 ? -80 : 20 * Mathf.Log10(volume);
        audioMixer.SetFloat("Effects Volume", volumeDecibels);
        PlayerPrefs.SetFloat("Volume", volume);
        Debug.Log("Volume changed to " + volume + "  and decibels:"  + volumeDecibels);
    }

    public void backButton() {
        escapeMenu.SetActive(true);
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("InSettingsMenu", 0);
    }


    public void changeKeybind(string keybind) {
        Debug.Log("Changing keybind for " + keybind);
        isWaitingForInput = true;
        keybindToChange = keybind;
    }

    private void Update()   {
        if (isWaitingForInput) {
            //GameObject.Find(keybindToChange + "Text").GetComponent<Text>().text = "......";
            foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if (isWaitingForInput == false) {
                    break;
                }
                GameObject.Find(keybindToChange + "Text").GetComponent<TMPro.TextMeshProUGUI>().text = "......";
                //checks if the input is a keycode
                if (Input.GetKey(keycode)) {
                    //set player prefs and change the keybind
                    PlayerPrefs.SetInt(keybindToChange, (int)keycode);
                    isWaitingForInput = false;
                    Debug.Log("Keybind changed to " + keycode.ToString());
                    GameObject.Find(keybindToChange + "Text").GetComponent<TMPro.TextMeshProUGUI>().text = keycode.ToString();
                }
            }
        }

        if (PlayerPrefs.GetInt("InSettingsMenu") == 0) {
            PlayerPrefs.SetInt("InSettingsMenu", 1);
        }

    }
}
