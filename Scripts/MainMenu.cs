using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public int playerHealth;

    public string startLevel;
    public string levelSelect;

    public static bool phoneMode = false;

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void NewGame() {
        SceneManager.LoadScene(startLevel);

        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetInt("PlayerMaxHealth", playerHealth);
    }

    public void LevelSelect() {
        SceneManager.LoadScene(levelSelect);

        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetInt("PlayerMaxHealth", playerHealth);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
