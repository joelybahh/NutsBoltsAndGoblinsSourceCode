//Name: BackGroundScroll.cs
//Author: AlySapphire  github.com/AlySapphire
//                     alyssafackrell.com
//Purpose Used for tiling clouds on the main menu screen to give it an animated effect

using UnityEngine;
using System.Collections;

public class BackGroundScroll : MonoBehaviour {

	public GameObject PanelA;
	public GameObject PanelB;
	public float ScrollSpeed;

	private float m_Dist;
	private Vector3 m_OrigPos;
	private Vector3 m_TempPos;

	// Use this for initialization
	void Start () {
		m_TempPos = new Vector3(0, 0, 0);
		m_OrigPos = new Vector3(PanelA.transform.position.x, PanelA.transform.position.y, PanelA.transform.position.z);
		m_Dist = Vector3.Distance(PanelA.transform.position, PanelB.transform.position); 
	}
	
	// Update is called once per frame
	void Update () {
		PanelA.transform.position = PanelA.transform.position + Vector3.left * ScrollSpeed * Time.deltaTime;
		PanelB.transform.position = PanelB.transform.position + Vector3.left * ScrollSpeed * Time.deltaTime;

		if(PanelA.transform.position.x < m_OrigPos.x - m_Dist) {
			m_TempPos = PanelA.transform.position;
			m_TempPos.x = PanelB.transform.position.x + m_Dist;
			PanelA.transform.position = m_TempPos;
		}
		if(PanelB.transform.position.x < m_OrigPos.x - m_Dist) {
			m_TempPos = PanelB.transform.position;
			m_TempPos.x = PanelA.transform.position.x + m_Dist;
			PanelB.transform.position = m_TempPos;
		}

	}

}
