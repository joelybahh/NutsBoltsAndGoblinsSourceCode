using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Load ();
	}
	
	public void Save () {
        BinaryFormatter binary = new BinaryFormatter ();
        FileStream fStream = File.Create (Application.persistentDataPath + "/saveFile.TPD");

        SaveManager saver = new SaveManager ();

		saver.levelStats = new LevelStats[LevelStatManager.Instance.levelStats.Length];

		// Save all completed Levels
		for(int i = 0; i < LevelStatManager.Instance.levelStats.Length; i++) {
			saver.levelStats[i] = LevelStatManager.Instance.levelStats[i];
		}

        binary.Serialize (fStream, saver);
        fStream.Close ();
    }

    public void Load () {
		if(File.Exists(Application.persistentDataPath + "/saveFile.TPD")) {
			BinaryFormatter binary = new BinaryFormatter();
			FileStream fStream = File.Open(Application.persistentDataPath + "/saveFile.TPD", FileMode.Open);
			SaveManager saver = (SaveManager)binary.Deserialize(fStream);
			fStream.Close();
			for(int i = 0; i < LevelStatManager.Instance.levelStats.Length; i++) {
                if(LevelStatManager.Instance.levelStats.Length > saver.levelStats.Length) {
                    ResetSave ();
                    break;
                }
                LevelStatManager.Instance.levelStats[i] = saver.levelStats[i];
            }
		} else {
			for(int i = 0; i < LevelStatManager.Instance.levelStats.Length; i++) {
				LevelStatManager.Instance.levelStats[i].bestHotStreak = 0;
				LevelStatManager.Instance.levelStats[i].starRating = 0;
				LevelStatManager.Instance.levelStats[i].wasFlawless = false;
				LevelStatManager.Instance.levelStats[i].wasCompleted = false;
			}			
		}
    }

	public void ResetSave() {
		for(int i = 0; i < LevelStatManager.Instance.levelStats.Length; i++) {
			LevelStatManager.Instance.levelStats[i].bestHotStreak = 0;
			LevelStatManager.Instance.levelStats[i].starRating = 0;
			LevelStatManager.Instance.levelStats[i].wasFlawless = false;
			LevelStatManager.Instance.levelStats[i].wasCompleted = false;
		}

		Save();
	}
}

[Serializable]
class SaveManager {
	public LevelStats[] levelStats;
}
