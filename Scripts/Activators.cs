using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activators : MonoBehaviour {

    public KeyCode key;

    private bool active = false;

    private GameObject note;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(key) && active) {
            Destroy(note.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        active = true;
        if (collision.gameObject.tag == "note") {
            note = collision.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        active = false;
    }

}
