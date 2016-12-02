using UnityEngine;
using System.Collections;

// <author>Joel Gabriel</author>
// <date>11/09/2016</date>
// <name>DisableOverTime.cs</name>
// <summary>Class that disables objects over time, a reiteration on destroy over time. This is used for object pools</summary>

// <TODO>Combine both the destoryOverTime, and this one to avoid redundant code</TODO>

public class DisableOverTime : MonoBehaviour {
    public float timeToDestroy = 5.0f;
    public float timeToFade = 1.0f;
    private float timer;
    private Renderer m_Rend;

    Color color = Color.white;

	public bool isBlood = true;

    void Start() {
        m_Rend = GetComponent<Renderer>();
    }

    void Update () {
		if (isBlood) {
			if (isActiveAndEnabled)
				timer += Time.deltaTime;
			if (timer >= timeToFade) {
				color.a -= 0.005f;          
			}
			if (color.a <= 0) {
				gameObject.SetActive (false);
				timer = 0.0f;
				color.a = 1;
			}
			if (m_Rend != null)
				m_Rend.material.color = color;
		} else {
			
			if(isActiveAndEnabled) timer += Time.deltaTime;
			if (timer >= timeToDestroy) {
				gameObject.SetActive (false);
				timer = 0.0f;
			}
		}
	}
}
