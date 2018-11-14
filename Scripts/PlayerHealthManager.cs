using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    public static int playerHealth;

    // Use this for initialization
    void Start() {
        playerHealth = PlayerPrefs.GetInt("PlayerHealth");
    }

    // Update is called once per frame
    void Update() {
        if (playerHealth <= 0) {
            gameObject.GetComponent<Animator>().SetTrigger("death");
        }
    }

    // Gives damage to game object
    public void giveDamage(int damageToGive) {
        playerHealth -= damageToGive;
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
    }

    // Set to full health
    public void setFullHealth() {
        playerHealth = PlayerPrefs.GetInt("PlayerMaxHealth", playerHealth);
    }
}
