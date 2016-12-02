using UnityEngine;
using System.Collections.Generic;

// <author>Joel Gabriel</author>
// <date>11/09/2016</date>
// <name>PaintBloodOnTerrain.cs</name>
/* <summary>If the name of the class isn't enough, this class gets a list of all the collision events from out particleSystem,
            pools them, and paints/repaints the "Paintable" tagged objects with blood.</summary>*/  

public class PaintBloodOnTerrain : MonoBehaviour {
    List<ParticleCollisionEvent> pcEvents = new List<ParticleCollisionEvent> ();
    public GameObject splat;
    public float yOffset;
    public int maxBloodOnScreen = 60;

    private ParticleSystem m_ParticleSystem;

    void Start() {
        PoolManager.Instance.CreatePool(splat, maxBloodOnScreen);
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }

	void OnParticleCollision(GameObject other ) {
        if (other.tag == "Paintable") {
            ParticlePhysicsExtensions.GetCollisionEvents (m_ParticleSystem, other, pcEvents);
            foreach (ParticleCollisionEvent p in pcEvents) {
                Vector3 curPos = new Vector3(p.intersection.x, p.intersection.y - yOffset, p.intersection.z);
                PoolManager.Instance.ReuseObject(splat, curPos, Quaternion.identity);
            }           
        }
    }
}
