using UnityEngine;
using System.Collections;

public class RotateFast : MonoBehaviour {
    public float speed = 10000;
	void Update () {
        transform.Rotate (Vector3.up, speed * Time.deltaTime);
	}
}
