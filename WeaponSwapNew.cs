// THIS ONE SWAPS WEAPONS

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class ReloadInfoNew {
	public int magSize = 10;
	public float fireRate = 0.25f;
	public float reloadTime = 1.5f;
}

[System.Serializable]
public class WeaponNew {
	public string Name;
	public GameObject WeaponObject;
	public eBULLETTYPE BulletType;
	public ReloadInfoNew WeaponMechanics;
}

public class WeaponSwapNew : MonoBehaviour {

	#region Public variables
	public WeaponNew[] Weapons;
	public ShootNew ShootScript;
	public float swapTime = 1.0f;
    public Image image;
    public Sprite uiWeaponA;
    public Sprite uiWeaponB;
	#endregion

	#region Private variables
	private int m_CurrWep;
    private float timer = 0.0f;
	private bool swapping = false;  
	#endregion

	public eBULLETTYPE CurrentWeapon {
		get { return Weapons[m_CurrWep].BulletType; }
	}

	public ReloadInfoNew CurrentMechanics {
		get { return Weapons[m_CurrWep].WeaponMechanics; }
	}

	public int CurrWepNum {
		get { return m_CurrWep; }
	}

	// Use this for initialization
	void Awake () {
		m_CurrWep = 0;
		for(int i = 0; i < Weapons.Length; i++) {
			Weapons[i].WeaponObject.SetActive(false);
		}
		Weapons[m_CurrWep].WeaponObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            
            swapping = true;
            //GetComponent<Animator> ().SetBool ("SwapWeaponAback", false);
            //GetComponent<Animator> ().SetBool ("SwapWeaponA", true);
        }

        if (swapping) {   
            timer += Time.deltaTime;
            if(timer >= swapTime) {
                //GetComponent<Animator> ().SetBool ("SwapWeaponA", false);
                //GetComponent<Animator> ().SetBool ("SwapWeaponAback", true);
                timer = 0.0f;
                SwapWeapon ();
                swapping = false;
            }
        }
        if (Weapons[m_CurrWep].Name == "WeaponA") {
            GetComponent<Animator> ().SetBool ("HoldingA", true);
            GetComponent<Animator> ().SetBool ("HoldingB", false);
        } else {
            GetComponent<Animator> ().SetBool ("HoldingA", false);
            GetComponent<Animator> ().SetBool ("HoldingB", true);
        }
    }

	public void SwapWeapon() {
        if (GameManager.Instance.OnTutorialLevel)
        {
            InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.NULL;
            InteractiveTutorial.Instance.messageThree.SetActive(false);
            InteractiveTutorial.Instance.canUpdateGameplay = true;
        }

        if (GameManager.Instance.tutTextTwoOn && GameManager.Instance.tutTextTwo != null) {
            GameManager.Instance.tutTextTwoOn = false;
            GameManager.Instance.tutTextTwo.SetActive (false);
        }

        if(Weapons[m_CurrWep].Name == "WeaponA") {
            GetComponent<Animator> ().SetTrigger ("SwapToB");
        } else if (Weapons[m_CurrWep].Name == "WeaponB") {
            GetComponent<Animator> ().SetTrigger ("SwapToA");
        }

        Weapons[m_CurrWep].WeaponObject.SetActive(false);
		m_CurrWep++;
		if(m_CurrWep == Weapons.Length) {
			m_CurrWep = 0;
		}
		Weapons[m_CurrWep].WeaponObject.SetActive(true);

        if(Weapons[m_CurrWep].Name == "WeaponA") {
            image.sprite = uiWeaponA;
            
        } else {
            image.sprite = uiWeaponB;
        }

		ShootScript.reloadText.SetActive(false);

	}

    public void SetButtonClickValues () {
        if (swapping != true) {
            swapping = true;
            if(InteractiveTutorial.Instance != null) InteractiveTutorial.Instance.canShoot = true;
            //GetComponent<Animator> ().SetBool ("SwapWeaponAback", false);
            //GetComponent<Animator> ().SetBool ("SwapWeaponA", true);
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
        }
        if (col.tag == "WeaponB")
        {
            Destroy(col.gameObject);
            InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.THIRD_MESSAGE;
            //SwapWeapon ();
        }
    }
}
