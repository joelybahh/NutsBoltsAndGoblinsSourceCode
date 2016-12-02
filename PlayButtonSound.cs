using UnityEngine;
using System.Collections;

public class PlayButtonSound : MonoBehaviour {
    AudioSource audioCenter;

    void Awake () {
        audioCenter = GameObject.Find ("AudioCentre").GetComponent<AudioSource> ();
    }

    public void PlaySound(AudioClip clip ) {
        audioCenter.PlayOneShot (clip);
    }

    public void GoToYouTube () {
        Application.OpenURL ("https://www.youtube.com/user/BlackPandaStudios1");
    }
    public void GoToTwitter () {
        Application.OpenURL ("https://www.twitter.com");
    }
    public void GoToFacebook () {
        Application.OpenURL ("https://www.facebook.com");
    }
}
