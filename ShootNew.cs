//Name: ShootNew.cs
//Author: AlySapphire  github.com/AlySapphire
//                     alyssafackrell.com
//Purpose: Old Mechanics. Handles bullet shooting and reloading

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShootNew : MonoBehaviour {

	#region public variables
	public GameObject BulletPrefab;
	public GameObject reloadText;
	public Transform ParentObj;
	public Transform GunAEndPosition;
	public Transform GunBEndPosition;
	public Material BulletAMaterial;
	public Material BulletBMaterial;
	public Text t_Ammo;
	public WeaponSwap wSwap;
	public int PoolSize;
	#endregion

	#region private variables
	private bool[] canShoot;
	private bool[] isReloaded;
	private int[] totalBullets;
	private float[] shootTimer;
	private float[] reloadTimer;
	private GameObject[] m_Pool;
	private int m_CurrPool;
	#endregion

	// Use this for initialization
	void Awake() {
		m_Pool = new GameObject[PoolSize];
		m_CurrPool = 0;
		int arrLen = wSwap.Weapons.Length;
		totalBullets = new int[arrLen];
		canShoot = new bool[arrLen];
		isReloaded = new bool[arrLen];
		shootTimer = new float[arrLen];
		reloadTimer = new float[arrLen];
		for(int i = 0; i < PoolSize; i++) {
			m_Pool[i] = Instantiate(BulletPrefab, Vector3.zero, BulletPrefab.transform.rotation) as GameObject;
			m_Pool[i].SetActive(false);
			m_Pool[i].transform.parent = ParentObj;
			if(i < totalBullets.Length) {
				totalBullets[i] = wSwap.CurrentMechanics.magSize;
				canShoot[i] = true;
				isReloaded[i] = true;
				shootTimer[i] = 0.0f;
				reloadTimer[i] = 0.0f;
			}
		}
	}

	// Update is called once per frame
	void Update() {
		// Reload
		int currWep = wSwap.CurrWepNum;
		if(totalBullets[currWep] <= 0) {
			canShoot[currWep] = false;
			isReloaded[currWep] = false;
			Reload(currWep);
		}

		// Fire rate
		shootTimer[currWep] += Time.deltaTime;
		if(shootTimer[currWep] >= wSwap.CurrentMechanics.fireRate) {
			shootTimer[currWep] = 0.0f;
			canShoot[currWep] = true;
		}
		if(GameManager.Instance.OnTutorialLevel) {
			if(Input.GetButtonDown("Fire1") && ( ( InteractiveTutorial.Instance.curTutState == InteractiveTutorial.eTutorialState.THIRD_MESSAGE || InteractiveTutorial.Instance.curTutState == InteractiveTutorial.eTutorialState.SECOND_MESSAGE || InteractiveTutorial.Instance.hasPlayedSecondMessage ) )) {
				ShootGun(currWep);
				InteractiveTutorial.Instance.hasPlayedSecondMessage = true;
				InteractiveTutorial.Instance.messageTwo.SetActive(false);
				InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.NULL;
			}
		} else {
			if(Input.GetButtonDown("Fire1"))
				ShootGun(currWep);
		}
		t_Ammo.text = totalBullets[currWep] + "/10";

	}

	public void ShootGun(int currWep) {
		if(WeaponSwapButton.CanShoot && canShoot[currWep] && isReloaded[currWep]) {
			if(GameManager.Instance.tutTextThreeOn && GameManager.Instance.tutTextThree != null) {
				GameManager.Instance.tutTextThree.SetActive(false);
			}
			totalBullets[currWep]--;
			m_Pool[m_CurrPool].GetComponent<Bullet>().Type = ( wSwap.CurrentWeapon == eBULLETTYPE.BULLETA ) ? eBULLETTYPE.BULLETA : eBULLETTYPE.BULLETB;
			m_Pool[m_CurrPool].transform.position = ( m_Pool[m_CurrPool].GetComponent<Bullet>().Type == eBULLETTYPE.BULLETA ) ? GunAEndPosition.transform.position : GunBEndPosition.transform.position;
			m_Pool[m_CurrPool].GetComponentInChildren<Renderer>().material = ( m_Pool[m_CurrPool].GetComponent<Bullet>().Type == eBULLETTYPE.BULLETA ) ? BulletAMaterial : BulletBMaterial;
			m_Pool[m_CurrPool].SetActive(true);
			m_CurrPool = ( m_CurrPool + 1 >= PoolSize ) ? 0 : m_CurrPool + 1;
			canShoot[currWep] = false;
		}

	}

	void Reload(int p_CurrWep) {
		reloadText.SetActive(true);
		reloadTimer[p_CurrWep] += Time.deltaTime;
		if(reloadTimer[p_CurrWep] >= wSwap.CurrentMechanics.reloadTime) {
			reloadTimer[p_CurrWep] = 0.0f;
			totalBullets[p_CurrWep] = wSwap.CurrentMechanics.magSize;
			isReloaded[p_CurrWep] = true;
			canShoot[p_CurrWep] = true;
			reloadText.SetActive(false);
		}
	}
}
