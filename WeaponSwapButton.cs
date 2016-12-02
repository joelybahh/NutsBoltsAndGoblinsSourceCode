using UnityEngine;
using System.Collections;

public class WeaponSwapButton : MonoBehaviour {

    public static bool CanShoot = true;

	void OnMouseEnter () {
        CanShoot = false; 
    }

    void OnMouseExit () {
        CanShoot = true;
    }

    public void SetCanShoot(bool value ) {
        CanShoot = value;
    }
}
