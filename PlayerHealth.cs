using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public int health;
    const int maxHealth = 3;
    public bool dead = false;

    public GameObject[] HealthBars = new GameObject[3];

	void Start () {
        health = maxHealth;
	}

    void Update () {
        if(health == 1) {
            HealthBars[0].SetActive (false);
            HealthBars[1].SetActive (false);
            HealthBars[2].SetActive (true);
        }
        if (health == 2) {
            HealthBars[0].SetActive (false);
            HealthBars[1].SetActive (true);
            HealthBars[2].SetActive (true);
        }
        if (health == 3) {
            HealthBars[0].SetActive (true);
            HealthBars[1].SetActive (true);
            HealthBars[2].SetActive (true);
        }
    }

    public void DeductHealth () {
        if (health > 1) health -= 1;
        else dead = true;
    }

    public void ResetHealth() {
        health = 0;
    }
}
