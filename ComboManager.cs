using UnityEngine;
using System.Collections;
using System;

// <author>Joel Gabriel</author>
// <date>11/09/2016</date>
// <name>ComboManager.cs</name>
// <summary>Class that manages all the logic for popup text effects</summary>

public class ComboManager : MonoBehaviour {

    private static ComboManager _instance;
    public static ComboManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ComboManager> ();
            }
            return _instance;
        }
        set { _instance = value; }
    }

	#region public variables (for designer)

    public GameObject t_Perfect;
    public GameObject t_Great;
    public GameObject t_Good;
    public GameObject t_OK;
    public GameObject t_Score;

    public float perfectThreshold;
    public float perfectScoreBonus;
    public float greatThreshold;
    public float greatScoreBonus;
    public float goodThreshold;
    public float goodScoreBonus;
    public float OkScoreBonus;
    public int One_TwoFiveStreak = 4;
    public int One_FiveStreak = 8;
    public int TwoTimesStreak = 12;
    public float textYOffset = 0.5f;

    public ParticleSystem pSystemA;
    public ParticleSystem pSystemB;

    #endregion

    #region public variables/properties (not for designer)

    public int curHotStreak { get; protected set; }
    public float multiplier {get; private set;}
	public Vector3 lastHitPos { get; set; }

    #endregion

    private Animator m_Animator;

    void Start () {
        multiplier = 1.0f;
 
		PoolManager.Instance.CreatePool (t_Perfect, 4, FindObjectOfType<Canvas> ().transform);
		PoolManager.Instance.CreatePool (t_Great, 4, FindObjectOfType<Canvas> ().transform);
		PoolManager.Instance.CreatePool (t_Good, 4, FindObjectOfType<Canvas> ().transform);
        PoolManager.Instance.CreatePool (t_Score, 4, FindObjectOfType<Canvas>().transform);
        PoolManager.Instance.CreatePool (t_OK, 4, FindObjectOfType<Canvas> ().transform);      

        m_Animator = GameManager.Instance.hotStreakT.gameObject.GetComponent<Animator>();
    }

    public void DetermineAndDisplayMessage (float distance) {
		/*		if you shoot the enemy within certain distance threshholds-----------------------------------------*/
        /**/	if (distance <= perfectThreshold)       PopupTextLogic (false, true, false, false, false, t_Perfect);
        /**/	else if (distance <= greatThreshold)    PopupTextLogic (false, false, true, false, false, t_Great);
        /**/	else if(distance <= goodThreshold)      PopupTextLogic (false, false, false, true, false, t_Good);
        /**/	else                                    PopupTextLogic (false, false, false, false, true, t_OK);
        /*		Setup and display popup text for that specific threshold-------------------------------------------*/
    }

    private void AwardHotStreakPoints(int curHotStreak ) {
        if (curHotStreak >= TwoTimesStreak) {
            multiplier = 2.0f;
        } else if (curHotStreak >= One_FiveStreak) {
            multiplier = 1.5f;
        } else if (curHotStreak >= One_TwoFiveStreak) {
            multiplier = 1.25f;
        } else {
            multiplier = 1.0f;
        }
    }

    public void ResetHotStreak () { curHotStreak = 0; GameManager.Instance.hotStrakCount = 0; }

    public void PopupTextLogic ( bool isDeflect, bool setPerfActive, bool setGreatActive, bool setGoodActive, bool setOkActive, GameObject t_Obj ) {
        
        t_Perfect.SetActive (setPerfActive);
        t_Great.SetActive (setGreatActive);
        t_Good.SetActive (setGoodActive);
        t_OK.SetActive (setOkActive);

        // TODO: grab an object from the pool and reposition. SetParent will occur on pool creation

        if (!isDeflect) {
            if (setPerfActive == false && setGreatActive == false) {
                if (curHotStreak >= 1)
                    m_Animator.SetTrigger("shake");
                curHotStreak = 0;
            } else {
                if (GameManager.Instance.tutText3D.activeInHierarchy)
                    GameManager.Instance.tutText3D.SetActive(false);

                m_Animator.SetTrigger("pop");           /* TODO: have a seperate popup for the score earnt off that enemy */
                curHotStreak++;
            }
        }

        AwardHotStreakPoints(curHotStreak);

        if (setPerfActive) {
            PoolManager.Instance.ReuseObject(t_Perfect, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z)), Quaternion.identity);
            GameManager.Instance.score += perfectScoreBonus * multiplier;
            PoolManager.Instance.ReuseTextAndReText(t_Score, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z - 0.8f)), Quaternion.identity, "+" + ((perfectScoreBonus * multiplier) * 100));
            pSystemA.Play ();
            pSystemB.Play ();
        } else if (setGreatActive) {
            PoolManager.Instance.ReuseObject(t_Great, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z)), Quaternion.identity);
            GameManager.Instance.score += greatScoreBonus * multiplier;
            PoolManager.Instance.ReuseTextAndReText(t_Score, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z - 0.8f)), Quaternion.identity, "+" + ((greatScoreBonus * multiplier) * 100));
            pSystemA.Play ();
            pSystemB.Play ();
        } else if (setGoodActive) {
            PoolManager.Instance.ReuseObject(t_Good, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z)), Quaternion.identity);
            GameManager.Instance.score += goodScoreBonus * multiplier;
            PoolManager.Instance.ReuseTextAndReText(t_Score, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z - 0.8f)), Quaternion.identity, "+" + ((goodScoreBonus * multiplier) * 100));
        } else if (setOkActive) {
            PoolManager.Instance.ReuseObject(t_OK, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z)), Quaternion.identity);
            PoolManager.Instance.ReuseTextAndReText(t_Score, Camera.main.WorldToScreenPoint(new Vector3(lastHitPos.x, lastHitPos.y + textYOffset, lastHitPos.z - 0.8f)), Quaternion.identity, "+" + ((OkScoreBonus * multiplier) * 100));
            GameManager.Instance.score += OkScoreBonus * multiplier;
        }
        
    }
}
