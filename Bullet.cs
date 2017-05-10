//Name: Bullet.cs
//Author: AlySapphire  github.com/AlySapphire
//                     alyssafackrell.com
//Purpose Handles Bullet movement and collision with other entities

using UnityEngine;
using System.Collections;

public enum eBULLETTYPE { BULLETA, BULLETB, BULLETC, BULLETD }

public class Bullet : MonoBehaviour {

	#region Public variables
	public float BulletSpeed;
	public float TimeTillDestroy;
    public float bulletKnockBack = 40;
    public float bulletKnockUp = 40;
    public float bulletKnockForward = 40;
    public bool TurnOnExperimentalManeuverability = true;
    public Vector3 BulletDirection;
    public GameObject t_Ouch;
	public AudioClip bloodHit;
	#endregion

	#region Private variables
	private float m_Timer;
	private bool m_Enabled;
	private bool m_IsDeflecting;
	private eBULLETTYPE m_Type;
    private Animator m_Shake;
    private PlayerHealth m_PlayHealth;
    private Rigidbody m_PlayRigBod;
    private Boss m_Boss;
    private EnemyBehaviour m_BossBehav;
	#endregion

	public eBULLETTYPE Type {
		get { return m_Type; }
		set { m_Type = value; }
	}

	// Use this for initialization
	void Start () {
		m_Timer = TimeTillDestroy;
		m_Enabled = false;
		m_IsDeflecting = false;
        m_Shake = GameManager.Instance.hotStreakT.gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if(isActiveAndEnabled) {
			if (m_IsDeflecting)
				transform.position -= BulletDirection * BulletSpeed * Time.deltaTime;
			else
				transform.position += BulletDirection * BulletSpeed * Time.deltaTime;
			m_Timer -= Time.deltaTime;
			if(m_Timer < 0.0f) {
				gameObject.SetActive(false);
				m_Timer = TimeTillDestroy;
                m_IsDeflecting = false;
            }
		}	
	}

	void BulletLogic(int p_EnemyNum, GameObject p_Other) {
		//Reset Timer
		m_Timer = TimeTillDestroy;

		//Run this code if we hit the enemy with the right kind of bullet
		if(p_EnemyNum == (int)m_Type) {
			//Play the correct Particle
			ParticleManager.Instance.RepositionAndPlay(( p_EnemyNum == 0 || p_EnemyNum % 2 == 0 ) ? "ParticleA" : "ParticleB", p_Other.transform.position);
			GameManager.Instance.audioSource.PlayOneShot (bloodHit);

			if(GameManager.Instance.hotStrakCount == GameManager.Instance.bestHotStreak) {
				GameManager.Instance.bestHotStreak++;
			}
			GameManager.Instance.hotStrakCount++;

			//Do this distance shit I'm not sure what it is
			float distFromEnemyToZone = Vector3.Distance(p_Other.transform.position, new Vector3(0, 0, GameManager.Instance.perfZone.position.z));
            Debug.Log (distFromEnemyToZone);

            ComboManager.Instance.lastHitPos = p_Other.transform.position;
            ComboManager.Instance.DetermineAndDisplayMessage(distFromEnemyToZone);

			//Destroy the enemy
			Destroy(p_Other.gameObject);
			//Set the bullet to inactive
			gameObject.SetActive(false);
			//Make sure we're not deflecting or whatever
			m_IsDeflecting = false;
		} else {    //Run this code if we hit the enemy with the wrong kind of bullet
			//Play the correct Particle
			ParticleManager.Instance.RepositionAndPlay("ParticleC", p_Other.transform.position);

            //Reset the streak or something
            if (ComboManager.Instance.curHotStreak >= 1) m_Shake.SetTrigger ("shake");
            ComboManager.Instance.ResetHotStreak();

            if (p_Other.tag != "EnemyA" && p_Other.tag != "EnemyC") {
                p_Other.transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("Deflect");
            }

            //Deflect the bullet
            m_IsDeflecting = true;
            ComboManager.Instance.lastHitPos = p_Other.transform.position;
            ComboManager.Instance.PopupTextLogic (true, false, false, false, false, t_Ouch);
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.CompareTag("EnemyA")) {
			BulletLogic(0, other.gameObject);
		} else if(other.collider.CompareTag("EnemyB")) {
			BulletLogic(1, other.gameObject);
		} else if(other.collider.CompareTag("EnemyC")) {
			BulletLogic(2, other.gameObject);
		} else if(other.collider.CompareTag("EnemyD")) {
			BulletLogic(3, other.gameObject);
		} else if(other.collider.CompareTag("Player")) {
			if(m_IsDeflecting) {
                if(m_PlayHealth == null) m_PlayHealth = other.gameObject.GetComponent<PlayerHealth>();
                if(m_PlayRigBod == null) m_PlayRigBod = other.gameObject.GetComponent<Rigidbody>();
                m_PlayHealth.DeductHealth();
                m_PlayRigBod.AddForce(new Vector3(transform.position.x, transform.position.y, bulletKnockBack * Time.deltaTime));
				gameObject.SetActive(false);
				m_IsDeflecting = false;
				m_Timer = TimeTillDestroy;
			} else {
				gameObject.SetActive(false);
				m_Timer = TimeTillDestroy;
			}
		} else if(other.collider.CompareTag("Boss")) {
            if(m_Boss == null) m_Boss = other.gameObject.GetComponent<Boss>();
            if(m_BossBehav == null) m_BossBehav = other.gameObject.GetComponent<EnemyBehaviour>();
            if (m_BossBehav.curBehaviour == EnemyBehaviour.eBehaviour.FALLBACK) {
                m_Timer = TimeTillDestroy;
                gameObject.SetActive (false);
                return;
            }
			int bullType = ( m_Type == eBULLETTYPE.BULLETA ) ? 1 : 2;
			Boss.eCurrentWeakness weak = m_Boss.Weakness;
			if(bullType == 1 && weak == Boss.eCurrentWeakness.TYPEA || bullType == 2 && weak == Boss.eCurrentWeakness.TYPEB) {
				m_Boss.DecreaseHealth();
				ParticleManager.Instance.RepositionAndPlay("ParticleA", other.transform.position);
			} else {
				ParticleManager.Instance.RepositionAndPlay("ParticleC", other.transform.position);
				float speed = m_BossBehav.speed;
				speed = speed * m_Boss.SpeedIncrease;
				m_BossBehav.speed += speed;
			}
			m_Timer = TimeTillDestroy;
			gameObject.SetActive(false);
		}

	}
}
