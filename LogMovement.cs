using UnityEngine;
using System.Collections;

public class LogMovement : MonoBehaviour {
	public float speed = 1.0f;
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.OnTutorialLevel) {
			if (InteractiveTutorial.Instance.canUpdateGameplay) {
				transform.position -= Vector3.forward * speed * Time.deltaTime;
			}
		} else {
			transform.position -= Vector3.forward * speed * Time.deltaTime;
		}
	}
}
