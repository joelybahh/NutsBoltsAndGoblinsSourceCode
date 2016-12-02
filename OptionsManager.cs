using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsManager : MonoBehaviour {

    public Toggle MusicTog;
    public Toggle SoundTog;
    public Toggle CCTog;
    public Toggle SSAOTog;
    public Toggle DOFTog;
    public Toggle SSTog;


    void Awake() {
        CCTog.isOn = (PlayerPrefs.GetString("CCOn") == "True") ? true : false;
        //Debug.Log(PlayerPrefs.GetString("CCOn"));
        DOFTog.isOn = (PlayerPrefs.GetString("DOFOn") == "True") ? true : false;
        SSTog.isOn = (PlayerPrefs.GetString("SSOn") == "True") ? true : false;
        SSAOTog.isOn = (PlayerPrefs.GetString("SSAOOn") == "True") ? true : false;
    }

	public void EnableMusic() {
        PlayerPrefs.SetString("MusicOn", MusicTog.isOn.ToString());
    }

    public void EnableSound() {
        PlayerPrefs.SetString("SoundOn", SoundTog.isOn.ToString());
    }

    public void EnableCC() {
        PlayerPrefs.SetString("CCOn", CCTog.isOn.ToString());
    }

    public void EnableSSAO() {
        PlayerPrefs.SetString("SSAOOn", SSAOTog.isOn.ToString());
    }

    public void EnableDOF() {
        PlayerPrefs.SetString("DOFOn", DOFTog.isOn.ToString());
    }

    public void EnableSS() {
        PlayerPrefs.SetString("SSOn", SSTog.isOn.ToString());
    }
}
