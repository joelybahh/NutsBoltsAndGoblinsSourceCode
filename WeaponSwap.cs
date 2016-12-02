// THIS ONE SWAPS WEAPONS

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class ReloadInfo {
	public int magSize = 10;
	public float fireRate = 0.25f;
	public float reloadTime = 1.5f;
}

[System.Serializable]
public class Weapon {
	public string Name;
	public GameObject WeaponObject;
	public eBULLETTYPE BulletType;
	public ReloadInfo WeaponMechanics;
}

public class WeaponSwap : MonoBehaviour {

	#region Public variables
	public Weapon[] Weapons;
	public Shoot ShootScript;
	public float swapTime = 1.0f;
    public Image image;
    public Sprite uiWeaponA;
    public Sprite uiWeaponB;
	#endregion

	#region Private variables
	private int m_CurrWep;
    private float timer = 0.0f;
	private bool swapping = false;
    private Animator m_Animator;  
	#endregion

	public eBULLETTYPE CurrentWeapon {
		get { return Weapons[m_CurrWep].BulletType; }
	}

	public ReloadInfo CurrentMechanics {
		get { return Weapons[m_CurrWep].WeaponMechanics; }
	}

	public int CurrWepNum {
		get { return m_CurrWep; }
	}

	// Use this for initialization
	void Awake () {
        m_Animator = GetComponent<Animator>();
		m_CurrWep = 0;
		for(int i = 0; i < Weapons.Length; i++) {
			Weapons[i].WeaponObject.SetActive(false);
		}
		Weapons[m_CurrWep].WeaponObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown (KeyCode.Q)) {
            SwapWeapon (0);
        } else if (Input.GetKeyDown (KeyCode.W)) {
            SwapWeapon (1);
        }

        if (Weapons[m_CurrWep].Name == "WeaponA") {
            m_Animator.SetBool ("HoldingA", true);
            m_Animator.SetBool ("HoldingB", false);
        } else if(Weapons[m_CurrWep].Name == "WeaponB") {
            m_Animator.SetBool ("HoldingA", false);
            m_Animator.SetBool ("HoldingB", true);
        } else if (Weapons[m_CurrWep].Name == "WeaponC") {
            m_Animator.SetBool ("HoldingB", false);
            m_Animator.SetBool ("HoldingA", true);
        } else {
            m_Animator.SetBool ("HoldingA", false);
            m_Animator.SetBool ("HoldingB", true);
        }
    }

	public void SwapWeapon(int swappingTo) {
        // TODO: if current weapon is equal to the button we pressed, don't swap
        if (m_CurrWep != swappingTo) {
            //StopCoroutine ("TimedShoot");
            if (GameManager.Instance.OnTutorialLevel) {
                InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.NULL;
                InteractiveTutorial.Instance.messageThree.SetActive (false);
                InteractiveTutorial.Instance.canUpdateGameplay = true;
                InteractiveTutorial.Instance.canShoot = true;
            }

            if (GameManager.Instance.tutTextTwoOn && GameManager.Instance.tutTextTwo != null) {
                GameManager.Instance.tutTextTwoOn = false;
                GameManager.Instance.tutTextTwo.SetActive (false);
            }

            if (Weapons[m_CurrWep].Name == "WeaponA") {
                m_Animator.SetTrigger ("SwapToB");
            } else if (Weapons[m_CurrWep].Name == "WeaponB") {
                m_Animator.SetTrigger ("SwapToA");
            }

            Weapons[m_CurrWep].WeaponObject.SetActive (false);

            m_CurrWep = swappingTo;
            Weapons[m_CurrWep].WeaponObject.SetActive (true);        

            ShootScript.reloadText.SetActive (false);

            StartCoroutine ("TimedShoot", swappingTo);
        
        } else {
            ShootScript.ShootGun (swappingTo);           
        }
    }

    IEnumerator TimedShoot (int wepToShoot) {
        yield return new WaitForSeconds(GameManager.Instance.shootDelay);
        ShootScript.ShootGun (wepToShoot);
        //yield return null;
    }

    public void SetButtonClickValues () {
        if (swapping != true) {
            swapping = true;
            if(InteractiveTutorial.Instance != null) InteractiveTutorial.Instance.canShoot = true;
        }
    }

    void OnTriggerEnter(Collider col ) {
        if(col.tag == "tutMessageTwo") {
            if (!GameManager.Instance.tutTextTwoOn) {
                GameManager.Instance.tutTextTwo.SetActive (true);
                GameManager.Instance.tutTextTwoOn = true;
            }
        }
        if (col.tag == "WeaponA")
        {
            Destroy(col.gameObject);
            InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.SECOND_MESSAGE;
            GameManager.Instance.canShootAfterFirstMessage = true;
        }
        if (col.tag == "WeaponB")
        {
            Destroy(col.gameObject);
            InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.THIRD_MESSAGE;
            //SwapWeapon ();
        }
    }
}
