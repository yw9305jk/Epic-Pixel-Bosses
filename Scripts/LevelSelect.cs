using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public int playerHealth;

    public string level1;
    public string level2;
    public string mainMenu;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SelectLevel1() {
        SceneManager.LoadScene(level1);

        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetInt("PlayerMaxHealth", playerHealth);
    }

    public void SelectLeve2() {
        SceneManager.LoadScene(level2);

        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetInt("PlayerMaxHealth", playerHealth);
    }

    public void SelectMainMenu() {
        SceneManager.LoadScene(mainMenu);
    }
}
