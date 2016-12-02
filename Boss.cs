using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemySpawnInfo {
	public int EnemySpawnNumber;
	public float TimeBeforeSpawn;
	public float DelayBetweenEnemies;
	public float PhaseEndWaitTime = 5.0f;
	public GameObject EnemyA;
	public GameObject EnemyB;
	public Transform SpawnPosition;
	public Transform PoolTransform;
}

[System.Serializable]
public class PhysicsSim {
	public float JumpDelay;
	public float FireAngle = 45.0f;
	public float Gravity = 9.8f;
	public Transform JumpTarget;
}

public class Boss : MonoBehaviour {

	public enum eCurrentWeakness {
		TYPEA,
		TYPEB,
	}

	#region Public Variables
	public GameObject BetweenScreen;
	public int InitialHealth = 50;
	[Range(0.01f, 1.0f)]
	public float SpeedIncrease;
	public float TimeBeforeSwap = 3.0f;
	public float ClawAnimLen = 3.0f;
	public float KnockbackForce;
	public float DistanceOfAttack;
	public EnemySpawnInfo SpawnInfo;
	public PhysicsSim JumpbackInfo;
	public eCurrentWeakness Weakness = eCurrentWeakness.TYPEA;

	//For testing purposes
	public Material TypeAMaterial;
	public Material TypeBMaterial;
	public SkinnedMeshRenderer Skin;
	#endregion

	#region Private Variables
	private EnemyBehaviour m_Ai;
	private Rigidbody m_RigBod;
	private Animator m_Animator;
	private int m_Health;
	private bool m_Switched;
	#endregion

	public bool Phase2 {
		get { return m_Switched; }
	}

	// Use this for initialization
	void Start () {
		m_Switched = false;
		m_Ai = GetComponent<EnemyBehaviour>();
		m_RigBod = GetComponent<Rigidbody>();
		m_Animator = transform.GetChild (0).GetComponent<Animator>();
		Skin.materials[0].color = Color.red;
		if(gameObject.GetComponent<Renderer>() != null) gameObject.GetComponent<Renderer>().material = SetMaterial();
		StartCoroutine("SwitchWeakness", TimeBeforeSwap);
		m_Health = InitialHealth;
		PoolManager.Instance.CreatePool(SpawnInfo.EnemyA, SpawnInfo.EnemySpawnNumber, SpawnInfo.PoolTransform);
		PoolManager.Instance.CreatePool(SpawnInfo.EnemyB, SpawnInfo.EnemySpawnNumber, SpawnInfo.PoolTransform);
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance(m_Ai.goal.transform.position, transform.position);
		if(dist < DistanceOfAttack) {
			m_Ai.SwitchState(EnemyBehaviour.eBehaviour.CLAW);
		}
	}

	Material SetMaterial() {
		switch(Weakness) {
			case eCurrentWeakness.TYPEA:
				return TypeAMaterial;
			case eCurrentWeakness.TYPEB:
				return TypeBMaterial;
		}

		return TypeAMaterial;
	}

	void SwapActiveColor() {
			Skin.materials[0].color = ( Weakness == eCurrentWeakness.TYPEB ) ? Color.blue : Color.red;
	}

	//Old weakness switch function. Relies on randomness
	IEnumerator SwitchWeaknessOLD(int p_Randomness = 10) {
		for(;;) {
			int randomValue = Random.Range(1, 100);

			if(randomValue % p_Randomness == 0) {
				Weakness = ( Weakness == eCurrentWeakness.TYPEA ) ? eCurrentWeakness.TYPEB : eCurrentWeakness.TYPEA;
                if (gameObject.GetComponent<Renderer>() != null) gameObject.GetComponent<Renderer>().material = ( Weakness == eCurrentWeakness.TYPEA ) ? TypeAMaterial : TypeBMaterial;
			}

			yield return new WaitForSeconds(0.5f);
		}

	}

	public IEnumerator SwitchWeakness(float p_TimeToWait) {
		for(;;) {
			Weakness = ( Weakness == eCurrentWeakness.TYPEA ) ? eCurrentWeakness.TYPEB : eCurrentWeakness.TYPEA;
			//if (gameObject.GetComponent<Renderer>() != null) gameObject.GetComponent<Renderer>().material = SetMaterial(); 
			SwapActiveColor();
			yield return new WaitForSeconds(p_TimeToWait);
		}
	}

	public void DecreaseHealth() {
		m_Health--;
		KnockBack();
		if(m_Health <= InitialHealth / 2 && !m_Switched) {
            m_Animator.SetTrigger ("KnockedBack");
			m_Ai.SwitchState(EnemyBehaviour.eBehaviour.FALLBACK);
            StopCoroutine ("SwitchWeakness");
            m_RigBod.useGravity = false;
			StartCoroutine(SimulateProjectile());
			m_Switched = true;
		}
		if(m_Health <= 0) {
            BetweenScreen.SetActive (true);
            BetweenScreen.GetComponent<Animator> ().SetTrigger ("SlideIn");
            Destroy (gameObject);
		}
	}

	void KnockBack() {
		m_RigBod.AddForce(Vector3.back * KnockbackForce, ForceMode.Impulse);
	}

	IEnumerator SimulateProjectile() {

		yield return new WaitForSeconds(JumpbackInfo.JumpDelay);

		//Calculate distance to target
		float targetDistance = Vector3.Distance(transform.position, JumpbackInfo.JumpTarget.position);

		//Calculate the velocity needed to throw the object to the target at specified angle
		float projectileVelocity = targetDistance / ( Mathf.Sin(2 * JumpbackInfo.FireAngle * Mathf.Deg2Rad) / JumpbackInfo.Gravity );

		//Extract the Y Z component of the velocity
		float vY = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(JumpbackInfo.FireAngle * Mathf.Deg2Rad);
		float vZ = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(JumpbackInfo.FireAngle * Mathf.Deg2Rad);

		//Calculate flight time
		float flightDuration = targetDistance / vZ;

		float elapse_Time = 0;

		while(elapse_Time < flightDuration) {
			//Move along the calculated curve
			transform.Translate(0, ( vY - ( JumpbackInfo.Gravity * elapse_Time ) ) * Time.deltaTime, -vZ * Time.deltaTime);
			elapse_Time += Time.deltaTime;
			yield return null;
		}
		//Debug.Log("Hit");
		//Turn gravity back on when we land so we don't slide
		m_RigBod.useGravity = true;
	}
}
