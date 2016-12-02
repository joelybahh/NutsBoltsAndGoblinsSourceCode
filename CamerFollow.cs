using UnityEngine;
using System.Collections;

// <author>Joel Gabriel</author>
// <date>11/09/2016</date>
// <name>CameraFollow.cs</name>
// <summary>Class that lerps towards a target position creating a smooth camera follow</summary>

public class CamerFollow : MonoBehaviour {

    public Transform target;
    public float damper = 10f;
	public float xCamPos = -1.44f;
	public float yCamPos = 2.2f;

	void Update () {
        transform.position = Vector3.Lerp (transform.position, new Vector3(xCamPos, yCamPos, target.position.z), damper * Time.deltaTime);
	}
}
