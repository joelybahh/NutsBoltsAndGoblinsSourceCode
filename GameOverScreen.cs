//Name: GameOverScreen.cs
//Author: AlySapphire  github.com/AlySapphire
//                     alyssafackrell.com
//Purpose Reloads the last played level or quits to main menu

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
