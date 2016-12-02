using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public struct LevelStats {
    public string name;
	public int bestHotStreak;
    public int starRating;
	public bool wasCompleted;
	public bool wasFlawless;
}

public class LevelStatManager : MonoBehaviour {

    private static LevelStatManager _instance;
    public static LevelStatManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<LevelStatManager> ();
            }
            return _instance;
        }
        set { _instance = value; }
    }

    public LevelStats[] levelStats;
	public int LastLoadedLevel;

    void Awake () {
        if(!_instance) {
            _instance = this;
        } else {
            Destroy (gameObject);
        }
        
        DontDestroyOnLoad (gameObject);
	}
}
