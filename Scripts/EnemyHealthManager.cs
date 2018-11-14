using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {

    public int bossHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(bossHealth <= 0) {
            gameObject.GetComponent<Animator>().SetTrigger("death");
        }
	}

    // Gives damage to game object
    public void giveDamage(int damageToGive) {
        bossHealth -= damageToGive;
    }
}
