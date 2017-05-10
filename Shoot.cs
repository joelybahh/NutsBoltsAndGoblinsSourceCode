//Name: Shoot.cs
//Author: AlySapphire  github.com/AlySapphire
//                     alyssafackrell.com
//Purpose: Handles the shooting mechanics

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class BulletInfo {
	public string Name;
	public GameObject BulletPrefab;
	public Transform SpawnPosition;
	public eBULLETTYPE BulletType;
}

public class Shoot : MonoBehaviour {

	#region public variables
	public BulletInfo[] BulletDetails;
    public GameObject reloadText;
    public Transform ParentObj;
    public Text t_Ammo;
	public WeaponSwap wSwap;
	public int PoolSize;

	public AudioClip shootSound;
	#endregion

	#region private variables
	private bool[] canShoot;
	private bool[] isReloaded;
    private int[] totalBullets;
    private float[] shootTimer;
    private float[] reloadTimer;
	private GameObject[,] m_Pool;
	private int[] m_CurrPool;
	#endregion

	// Use this for initialization
	void Awake () {
		m_Pool = new GameObject[BulletDetails.Length, PoolSize];
		int arrLen = wSwap.Weapons.Length;
		totalBullets = new int[arrLen];
		canShoot = new bool[arrLen];
		isReloaded = new bool[arrLen];
		shootTimer = new float[arrLen];
		reloadTimer = new float[arrLen];
		m_CurrPool = new int[arrLen];
		for(int i = 0; i < totalBullets.Length; i++) {
			m_CurrPool[i] = 0;
			totalBullets[i] = wSwap.CurrentMechanics.magSize;
			canShoot[i] = true;
			isReloaded[i] = true;
			shootTimer[i] = 0.0f;
			reloadTimer[i] = 0.0f;
		}
		for(int bullet = 0; bullet < BulletDetails.Length; bullet++) {
			for(int Pool = 0; Pool < PoolSize; Pool++) {
				m_Pool[bullet, Pool] = Instantiate(BulletDetails[bullet].BulletPrefab, Vector3.zero, BulletDetails[bullet].BulletPrefab.transform.rotation) as GameObject;
				m_Pool[bullet, Pool].SetActive(false);
				m_Pool[bullet, Pool].transform.parent = ParentObj;
				m_Pool[bullet, Pool].GetComponent<Bullet>().Type = BulletDetails[bullet].BulletType;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Reload
		int currWep = wSwap.CurrWepNum;
        if (totalBullets[currWep] <= 0) {
            canShoot[currWep] = false;
            isReloaded[currWep] = false;
            Reload (currWep);
        }

        // Fire rate
        shootTimer[currWep] += Time.deltaTime;
        if (shootTimer[currWep] >= wSwap.CurrentMechanics.fireRate) {
            shootTimer[currWep] = 0.0f;
            canShoot[currWep] = true;
        }

    }

    public void ShootGun (int currWep) {
        if (GameManager.Instance.canShootAfterFirstMessage) {
            if (GameManager.Instance.audioSource != null) GameManager.Instance.audioSource.PlayOneShot (shootSound);
            if (WeaponSwapButton.CanShoot && canShoot[currWep] && isReloaded[currWep]) {
                if (GameManager.Instance.tutTextThreeOn && GameManager.Instance.tutTextThree != null) {
                    GameManager.Instance.tutTextThree.SetActive (false);
                }

                // TODO: wait for animation to finish before shooting



                totalBullets[currWep]--;
                m_Pool[currWep, m_CurrPool[currWep]].transform.position = BulletDetails[currWep].SpawnPosition.position;
                m_Pool[currWep, m_CurrPool[currWep]].SetActive (true);
                m_CurrPool[currWep] = (m_CurrPool[currWep] + 1 >= PoolSize) ? 0 : m_CurrPool[currWep] + 1;

                canShoot[currWep] = false;
            }
        }

    }

    public void EndShootTutorial () {
        InteractiveTutorial.Instance.hasPlayedSecondMessage = true;
        InteractiveTutorial.Instance.messageTwo.SetActive (false);
        InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.NULL;
    }

    void Reload(int p_CurrWep) {
        reloadText.SetActive (true);
        reloadTimer[p_CurrWep] += Time.deltaTime;
		if(reloadTimer[p_CurrWep] >= wSwap.CurrentMechanics.reloadTime) {
			reloadTimer[p_CurrWep] = 0.0f;
			totalBullets[p_CurrWep] = wSwap.CurrentMechanics.magSize;
			isReloaded[p_CurrWep] = true;
			canShoot[p_CurrWep] = true;
            reloadText.SetActive (false);
        }
    }
}
