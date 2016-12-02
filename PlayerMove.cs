using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerMove : MonoBehaviour {

	#region Public Variables
	public Transform goal;    
    public float speed = 10.0f;
    public GameObject betweenObj;
    public float curForce;
    public float bulletKnockUp;
    public float bulletKnockForward;
    public int starRating;
    public float swipeThreshold = 5.0f;
    public float atkTime = 3.0f;
	#endregion

	#region Private Variables
	private Rigidbody rb;
	private PlayerHealth playerHealth;
	private bool hasJumped = false;
	private float timer = 0.0f;
    private bool colliding = false;
    private Vector3 intialClick;
    private Vector3 releaseClick;
	#endregion


	public bool IsColliding {
        get { return colliding; }
        set { colliding = value; }
    }

	void Start () {
        rb = GetComponent<Rigidbody> ();
        playerHealth = GetComponent<PlayerHealth> ();
	}
	
	void Update () {
        if (GameManager.Instance.OnTutorialLevel) {
            if (InteractiveTutorial.Instance.canUpdateGameplay){
                timer += Time.deltaTime;
                if (!colliding) transform.position += transform.forward * speed * Time.deltaTime;
                else {
                    if (timer >= atkTime) {
                        rb.AddForce(new Vector3(transform.position.x, transform.position.y, 1 * curForce));
                        playerHealth.DeductHealth();
                        timer = 0;
                    }
                }
                if (playerHealth.dead) {
					LevelStatManager.Instance.LastLoadedLevel = SceneManager.GetActiveScene().buildIndex;
					SceneManager.LoadScene(8);
                    //TODO: Go to between screen, but death version
                }
            }
        }
    }

    void OnCollisionEnter(Collision col ) {
		EnemyColInOut (col, true, false);
    }

    void OnCollisionExit ( Collision col ) {
        if (col.transform.tag == "EnemyA" || col.transform.tag == "EnemyB" || col.transform.tag == "EnemyC"  || col.transform.tag == "PlatformSpike") {
            colliding = false;
            col.gameObject.GetComponent<EnemyBehaviour> ().CanMove = true;
        }
    }

	void OnTriggerEnter(Collider col){
		if (col.tag == "LevelSwap") {       
            betweenObj.SetActive (true);
            betweenObj.GetComponent<Animator> ().SetTrigger ("SlideIn");
            starRating = GameManager.Instance.GetStarRating (1, GameManager.Instance.score);
        }
        if(col.tag == "tutMessageOne") {
            InteractiveTutorial.Instance.curTutState = InteractiveTutorial.eTutorialState.FIRST_MESSAGE;
        }
	}

	public void Knockback() {
		rb.AddForce(new Vector3(transform.position.x, transform.position.y, 1 * curForce));
		playerHealth.DeductHealth();
		timer = 0;
	}

	private void EnemyColInOut(Collision col, bool isColliding, bool canMove){
		if (col.transform.tag == "EnemyA" || col.transform.tag == "EnemyB" || col.transform.tag == "EnemyC"  || col.transform.tag == "PlatformSpike") {
			colliding = isColliding;
			col.gameObject.GetComponent<EnemyBehaviour> ().CanMove = canMove;
		}
	}
}
