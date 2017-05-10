//Name: FloatEffect.cs
//Author: AlySapphire  github.com/AlySapphire
//                     alyssafackrell.com
//Purpose Used to create the floating effect of the Robot on the main menu

using UnityEngine;
using System.Collections;

public class FloatEffect : MonoBehaviour {

	public float Speed;
	public float Amplitude;
	public float AmplitudeMobile;

	private float m_OrigYPos;
	private Vector3 temp;

	// Use this for initialization
	void Start () {
		temp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		m_OrigYPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		temp = transform.position;
		temp.y = m_OrigYPos + ((Application.platform == RuntimePlatform.Android) ? AmplitudeMobile : Amplitude) * Mathf.Sin(Speed * Time.time);
		transform.position = temp;
		//transform.position.y = m_OrigYPos +
	}
}
