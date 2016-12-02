using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	public Image splashImage;
	public float onScreenTime = 3.0f;
	Color color = new Color(1,1,1,0);

	void Start () {
		StartCoroutine ("FadeOutIn");
	}
	
	private IEnumerator FadeOutIn(){
		// FADE IN
		while (splashImage.color.a <= 1) {
			color.a += 1f * Time.deltaTime;
			splashImage.color = color;
			yield return null;
		}

		// WAIT
		yield return new WaitForSeconds (onScreenTime);


		// FADE OUT
		while (splashImage.color.a >= 0) {
			color.a -= 1f * Time.deltaTime;
			splashImage.color = color;
			yield return null;
		}

		//GET OUUTTTAA THERE
		SceneManager.LoadScene (1);
	}
}
