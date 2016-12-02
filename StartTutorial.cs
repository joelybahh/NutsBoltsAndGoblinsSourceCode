using UnityEngine;
using System.Collections;

public class StartTutorial : MonoBehaviour {
    public GameObject tutObj;
	void Start () {
        //GameManager.Instance.PauseGame ();
	}

    public void ProgressPastTutorial () {
        GameManager.Instance.PauseGame ();
        tutObj.SetActive (false);
    }
}
