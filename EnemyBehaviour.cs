using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public bool DisableAi = false;

    public enum eBehaviour {
        RUNNING,
        ATTACK,
        ENRAGED,
        DIE,
        COUNT,
		APPROACH,
		FALLBACK,
		CLAW
    };

	#region Public Variables
	public eBehaviour curBehaviour = eBehaviour.RUNNING;
    public PlayerMove pMove;
    public float speed = 10.0f;
    public GameObject goal;
    public bool CanMove = true;
	public float TimeEnraged = 1.5f;
    public Renderer zoneRenderer;
	#endregion

	#region Private Variables
	private float timer = 0.0f;
	private float m_InitSpeed;
	private bool m_WaveStarted;
	private bool m_Done;
	private bool m_FirstPass;
	private bool m_Running;
	private bool m_Attkd;
	private bool m_StopFuckingUp;
    private Animator m_Animator;
    private Animator m_BossAnimator;
    private Boss m_Boss;
    private PlayerMove m_PlayMov;
	#endregion

	void Start() {
		m_InitSpeed = speed;
		m_WaveStarted = false;
		m_Done = false;
		m_FirstPass = true;
		m_Running = false;
		m_Attkd = false;
		m_StopFuckingUp = false;
        m_Animator = (gameObject.tag == "EnemyA") ? GetComponent<Animator>() : transform.GetChild(0).GetComponent<Animator>();
        m_BossAnimator = transform.GetChild (0).GetComponent<Animator> ();
        m_Boss = GetComponent<Boss>();
        if(goal != null) m_PlayMov = goal.GetComponent<PlayerMove>();
	}

    void Update () {
        switch (curBehaviour) {
            case eBehaviour.RUNNING:    UpdateRun ();			break;
            case eBehaviour.ATTACK:     UpdateAttack ();        break;
            case eBehaviour.ENRAGED:    UpdateEnragedRun ();    break;
            case eBehaviour.DIE:        UpdateDie ();           break;
			case eBehaviour.APPROACH:	UpdateApproach();		break;
			case eBehaviour.FALLBACK:	UpdateFallback();		break;
			case eBehaviour.CLAW:		UpdateClaw();			break;
		}

        if(m_Animator != null) m_Animator.enabled = (InteractiveTutorial.Instance != null && InteractiveTutorial.Instance.canUpdateGameplay == false) ? false : true;
    }

    public void SwitchState(eBehaviour newState ) {
		if(!DisableAi) {
			curBehaviour = newState;
		}
    }

    private void UpdateRun () {
		if (InteractiveTutorial.Instance.canUpdateGameplay) 
			if (CanMove)
				transform.position -= -Vector3.forward * speed * Time.deltaTime;
	}
    
    private void UpdateEnragedRun () {
		if (InteractiveTutorial.Instance.canUpdateGameplay) {
			timer += Time.deltaTime;
			if (CanMove)
				transform.position -= -Vector3.forward * (speed + (speed) * 2) * Time.deltaTime;
			if (timer >= TimeEnraged) {
				timer = 0.0f;
				SwitchState (eBehaviour.RUNNING);
			}
		}
    }
    private void UpdateAttack ( bool aIsEnraged = false ) { }
    private void UpdateDie () { }

	private void UpdateApproach() {
		if(m_StopFuckingUp) {
			SwitchState(eBehaviour.FALLBACK);
            return;
		}

        if (CanMove)
			transform.position -= -Vector3.forward * ((m_Boss.Phase2 == true) ? speed * 2 : speed) * Time.deltaTime;
	}
	private void UpdateFallback() {
		/*
		 TODO: Add code for Fallback behaviour
			-Play jumping back animation
		*/
		m_StopFuckingUp = true;
        GameManager.Instance.perfZone.gameObject.SetActive (true);

		if(m_Running) {
			StopCoroutine(DelaySwitch());
			m_Running = false;
		}	

		if(!m_WaveStarted) {
			StartCoroutine(SpawnEnemies());
			m_WaveStarted = true;
		}
		if(m_Done) {
			StopCoroutine(SpawnEnemies());
			speed = m_InitSpeed;
			SwitchState(eBehaviour.APPROACH);
            StartCoroutine (m_Boss.SwitchWeakness (m_Boss.TimeBeforeSwap));
            m_BossAnimator.SetTrigger ("StartApproach");
            m_StopFuckingUp = false;
            GameManager.Instance.perfZone.gameObject.SetActive (false);
        }
	}
	private void UpdateClaw() {
		/*
		 TODO: Add code for Claw behaviour
			-Play Animation
		*/
		if(!m_Attkd) {
			m_PlayMov.Knockback();
			//Alyssa's way
			speed = m_InitSpeed;
			//Joel's way
			//speed = speed / m_Boss.SpeedIncrease;
			StartCoroutine(DelaySwitch());
			m_Attkd = true;
		}
	
	}

	void OnDestroy () {
        if(pMove != null) pMove.IsColliding = false;
        if(zoneRenderer != null) zoneRenderer.material.color = Color.white;
    }

	IEnumerator SpawnEnemies() {
		for(int i = 0; i < m_Boss.SpawnInfo.EnemySpawnNumber; i++) {
			if(m_FirstPass) {
				m_FirstPass = false;
				i = 0;
				yield return new WaitForSeconds(m_Boss.SpawnInfo.TimeBeforeSpawn);
			}

            m_BossAnimator.SetTrigger ("SendGobbies");

			int value = Random.Range(0, 2);
			if(value == 0) {
                m_Boss.Skin.materials[0].color = Color.red;
                PoolManager.Instance.ReuseObject(m_Boss.SpawnInfo.EnemyA, m_Boss.SpawnInfo.SpawnPosition.position, m_Boss.SpawnInfo.EnemyA.transform.rotation);  
			} else if(value == 1) {
                m_Boss.Skin.materials[0].color = Color.blue;
                PoolManager.Instance.ReuseObject(m_Boss.SpawnInfo.EnemyB, m_Boss.SpawnInfo.SpawnPosition.position, m_Boss.SpawnInfo.EnemyB.transform.rotation);
			} 
			if(i == m_Boss.SpawnInfo.EnemySpawnNumber - 1)
				yield return new WaitForSeconds(m_Boss.SpawnInfo.PhaseEndWaitTime);
			yield return new WaitForSeconds(m_Boss.SpawnInfo.DelayBetweenEnemies);
		}
		m_Done = true;
	}

	IEnumerator DelaySwitch() {
		m_Running = true;
		yield return new WaitForSeconds(m_Boss.ClawAnimLen);
		if(!m_StopFuckingUp)
			SwitchState(eBehaviour.APPROACH);
		m_Attkd = false;
	}

    void OnTriggerStay(Collider other ) {
        if(other.tag == "PerfZone") {
            // SET TO GREEN
            zoneRenderer.material.color = Color.green;
        }
    }

    void OnTriggerExit ( Collider other ) {
        if (other.tag == "PerfZone") {
            // SET TO RED
            zoneRenderer.material.color = Color.white;
        }
    }
    
}
