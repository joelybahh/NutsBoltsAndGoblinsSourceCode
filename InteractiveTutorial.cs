using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InteractiveTutorial : MonoBehaviour {

    private static InteractiveTutorial _instance;
    public static InteractiveTutorial Instance {
        get {
            if (_instance == null) _instance = FindObjectOfType<InteractiveTutorial>();
            return _instance;
        }
        set { _instance = value; }
    }

    public enum eTutorialState {
        NULL = -1,
        FIRST_MESSAGE,
        SECOND_MESSAGE,
        THIRD_MESSAGE,
        COUNT
    }

    public eTutorialState curTutState = eTutorialState.NULL;

    #region Too many public GameObjects :^)

    public GameObject messageOne;
    public GameObject messageTwo;
    public GameObject messageThree;

    //public GameObject jumpButton;
    //public GameObject swapButton;

    public GameObject weaponA;
    public GameObject weaponB;

    #endregion

    public bool canUpdateGameplay = true;
    public bool canShoot = false;
    public bool hasPlayedSecondMessage = false;

    void Start()
    {
        switch(SceneManager.GetActiveScene ().buildIndex) {
            case 1:
                messageOne.SetActive    (false);
                messageTwo.SetActive    (false);
                messageThree.SetActive  (false);
                weaponA.SetActive       (false);
                weaponB.SetActive       (false);
                break;
            case 2:
                messageOne.SetActive    (false);
                messageTwo.SetActive    (false);
                messageThree.SetActive  (false);
                weaponA.SetActive       (false);
                weaponB.SetActive       (false);
                break;
            case 3:
                messageOne.SetActive    (false);
                messageTwo.SetActive    (false);
                messageThree.SetActive  (false);
                weaponA.SetActive       (true);
                weaponB.SetActive       (false);
                break;
        }
    }

    void Update()
    {
        switch (curTutState)
        {
            case eTutorialState.NULL:
                canUpdateGameplay = true;
                break;
            case eTutorialState.FIRST_MESSAGE:
                canUpdateGameplay = false;
                SetAllFalseExcept(messageOne);
                //jumpButton.SetActive(true);
                break;
            case eTutorialState.SECOND_MESSAGE:
                //canShoot = true;
                canUpdateGameplay = false;
                SetAllFalseExcept(messageTwo);
                //weaponA.SetActive(true);
                break;
            case eTutorialState.THIRD_MESSAGE:
                //canShoot = true;
                canUpdateGameplay = false;
                messageThree.SetActive(true);
                SetAllFalseExcept(messageThree);
                break;
        }
    }

    void SetAllFalseExcept(GameObject objToKeepTrue)
    {
        if (messageOne != objToKeepTrue) messageOne.SetActive(false);
        else messageOne.SetActive(true);

        if (messageTwo != objToKeepTrue) messageTwo.SetActive(false);
        else messageTwo.SetActive(true);

        if (messageThree != objToKeepTrue) messageThree.SetActive(false);
        else messageThree.SetActive(true);

    }

    public void FirstTutConditionMet(){
        if (curTutState == eTutorialState.FIRST_MESSAGE){
            curTutState = eTutorialState.NULL;
            messageOne.SetActive(false);
        }
    }
}
