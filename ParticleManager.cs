// THIS ONE MANAGES PARTICLES

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Particlez {
	public string Name;
	public GameObject ParticleEffect;
}

public class ParticleManager : MonoBehaviour {

	#region Public variables
	public Particlez[] ParticleArray;
	public Transform ParentObj;
	#endregion

	#region Private variables
	private GameObject[] m_Particles;
	private static ParticleManager m_Instance;
    private ParticleSystem[] m_ParticleSystems;
	#endregion

	public static ParticleManager Instance {
		get {
			if(m_Instance != null) {
				return m_Instance;
			}
			return m_Instance;
		}
	}

	void Awake() {
		m_Instance = this;
		m_Particles = new GameObject[ParticleArray.Length * 3];
        m_ParticleSystems = new ParticleSystem[ParticleArray.Length * 3];

		for(int i = 0; i < m_Particles.Length; i++) {
			m_Particles[i] = Instantiate(ParticleArray[(i == 0) ? i : i / 3].ParticleEffect, new Vector3(0, 0, 0), ParticleArray[(i == 0) ? i : i / 3].ParticleEffect.transform.rotation) as GameObject;
            m_ParticleSystems[i] = m_Particles[i].GetComponent<ParticleSystem>();
            m_ParticleSystems[i].Stop();
			if(ParentObj != null) {
				m_Particles[i].transform.parent = ParentObj;
			}
		}
	}

	public void RepositionAndPlay(string _name, Vector3 _newPos) {
		int index = -1;
        
        for (int i = 0; i < m_Particles.Length; i++) {
			if(ParticleArray[(i == 0) ? i : i / 3].Name == _name) {
				if(m_ParticleSystems[i].isPlaying) {
					continue;
				}
				index = i;
				break;
			}
		}

		if(index == -1)		return;

       

        m_Particles[index].transform.position = _newPos;
        m_ParticleSystems[index].Stop();
        m_ParticleSystems[index].Play();
	}
}
