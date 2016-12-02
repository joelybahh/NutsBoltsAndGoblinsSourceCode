using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BetweenScreen : MonoBehaviour {

    public Text t_HeartsRemaining;
    public Text t_ElapsedTime;
    public Text t_BestTime;

    public SaveLoad sl;

    public int currentLevelIndex = 0;

    public Image[] stars = new Image[3];
    public PlayerMove pMove;

	void Start () {
        sl = FindObjectOfType<SaveLoad>();
        if(pMove.starRating == 3) {
            for(int i = 0; i < 3; i++) {
                stars[i].color = Color.white;
            }
        } else if(pMove.starRating == 2) {
            for (int i = 0; i < 2; i++) {
                stars[i].color = Color.white;
            }
        } else {
            stars[0].color = Color.white;
        }

        t_HeartsRemaining.text = "     Hearts Remaining: " + pMove.gameObject.GetComponent<PlayerHealth> ().health;
        t_ElapsedTime.text = "     Score: " + GameManager.Instance.score * 100;
        t_BestTime.text = "     Best Streak: " + GameManager.Instance.bestHotStreak;

		statSetter();

		// These 2 combined took half a second to process, that doesnt even include all the other stuff in the profiler
		sl.Save();		// TODO: if these started coroutines that skipped a frame
        sl.Load ();		// TODO: we would greatly improve performance
    }

	void statSetter() {
		int currScene = SceneManager.GetActiveScene().buildIndex - 2;

		LevelStatManager.Instance.levelStats[currScene].wasCompleted = true;
		if(LevelStatManager.Instance.levelStats[currScene].starRating < pMove.starRating) {
			LevelStatManager.Instance.levelStats[currScene].starRating = pMove.starRating;
		}
		
	}
}
