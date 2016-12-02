using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// <author>Joel Gabriel</author>
// <date>11/09/2016</date>
// <name>GameManager.cs</name>
// <summary>Class that manages gameplay related logic, and acts as a return point from other scripts</summary>

[System.Serializable]
public struct StarRating {
    public string m_Name;
    public Condition m_Conditions;
}

[System.Serializable]
public struct Condition {
    public string m_name;
    public float score;
}

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
        set { _instance = value; }       
    }

    public GameObject PauseScreen;
    public int starsCollected;
	public bool OnTutorialLevel;

    public StarRating[] rating = new StarRating[3]; // TODO: change star rating logic, remove old time based logic

    #region tutLevel
    public GameObject tutTextOne;
    public GameObject tutTextTwo;
    public GameObject tutTextThree;
    public GameObject tutTextFour;
    public bool tutTextOneOn = true;
    public bool tutTextTwoOn = false;
    public bool tutTextThreeOn = false;
    #endregion

    public bool paused = false;
    public Transform perfZone;
    public float score { get; set; }
    public Text scoreT;
    public Text hotStreakT;
    public Text multiplierT;
    public float shootDelay = 0.05f;
    public int bestHotStreak;
    public GameObject tutText3D;
	public int hotStrakCount;

    public bool canShootAfterFirstMessage = false;

	public AudioSource audioSource { get; private set; }

	void Awake(){		
		audioSource = GameObject.Find ("AudioCentre").GetComponent<AudioSource>();
        Camera.main.GetComponent<ColorCorrectionCurves>().enabled = (PlayerPrefs.GetString("CCOn") == "True") ? true : false;
        Camera.main.GetComponent<DepthOfField>().enabled = (PlayerPrefs.GetString("DOFOn") == "True") ? true : false;
        Camera.main.GetComponent<SunShafts>().enabled = (PlayerPrefs.GetString("SSOn") == "True") ? true : false;
        Camera.main.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = (PlayerPrefs.GetString("SPAOOn") == "True") ? true : false;
		hotStrakCount = 0;

        

        if(Application.loadedLevel == 2) {
            canShootAfterFirstMessage = false;
        } else {
            canShootAfterFirstMessage = true;
        }
        // TODO: 2 seperate audioSources so I can mute the sound effects and music seperately based off the sound settings
    }

    void Update() {
        scoreT.text = "Score: " + Mathf.RoundToInt(score * 100);
        hotStreakT.text = "Hot Streak: " + ComboManager.Instance.curHotStreak;
        multiplierT.text = "x" + ComboManager.Instance.multiplier;
    }

    public int GetStarRating(int a_LevelIndex, float a_score) {
        int starRating = 1;

        for(int i = 0; i < 3; i++) {
            if(a_score * 100 >= rating[i].m_Conditions.score) {
                if      (i == 0) return starRating = 3;
                else if (i == 1) return starRating = 2;
                else             return starRating = 1;
            }
        }

        return starRating = 1;
    }

    public void PauseGame () {
        if (paused) {
            PauseScreen.SetActive (false);
            Time.timeScale = 1.0f;
            paused = false;
            WeaponSwapButton.CanShoot = true;
            if(tutTextFour != null && tutTextFour.activeSelf == true) {
                tutTextFour.SetActive (false);
            }
        } else if (!paused) {
            PauseScreen.SetActive (true);
            Time.timeScale = 0.0f;
            paused = true;
        }
    }
}
