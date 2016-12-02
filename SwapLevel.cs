using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwapLevel : MonoBehaviour {

	public int levelToSwapTo;

	public void SwapToLevel(){
		SceneManager.LoadScene (levelToSwapTo);
	}
}
