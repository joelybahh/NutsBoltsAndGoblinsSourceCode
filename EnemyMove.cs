using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    public float speed = 10.0f;
    public bool CanMove = true;

	void Update () {
        if(CanMove) transform.position -= Vector3.forward * speed * Time.deltaTime;
    }
}
