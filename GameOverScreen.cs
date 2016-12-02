using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

	public void RetryLevel() {
		SceneManager.LoadScene(LevelStatManager.Instance.LastLoadedLevel);
	}

	public void MainMenu() {
		SceneManager.LoadScene(1);
	}
}
