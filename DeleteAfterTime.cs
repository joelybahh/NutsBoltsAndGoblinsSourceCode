using UnityEngine;
using System.Collections;

// <author>Joel Gabriel</author>
// <date>11/09/2016</date>
// <name>DestroyOverTime.cs</name>
// <summary>Class that Destroys an gameobject after a certain time is passed</summary>

public class DeleteAfterTime : MonoBehaviour {
    float timer = 0.0f;
    public float deleteTime = 2f;

	void Update () {
        timer += Time.deltaTime;
        if(timer >= deleteTime) {
            Destroy (gameObject);
        }
	}
}
