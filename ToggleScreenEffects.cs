using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class ToggleScreenEffects : MonoBehaviour {

    #region Public Variables
    public Toggle BloomTog;
    public Toggle DofTog;
    public Toggle AmbientOccTog;
    public Toggle SunShaftsTog;
    public Toggle CcTog;
    #endregion

    #region Private Variables
    private BloomOptimized m_Bloom;
    private DepthOfField m_Depth;
    private ScreenSpaceAmbientOcclusion m_SSAO;
    private SunShafts m_SunShafts;
    private ColorCorrectionCurves m_CCC;
    #endregion

    void Awake() {
        m_Bloom = Camera.main.GetComponent<BloomOptimized>();
        m_Depth = Camera.main.GetComponent<DepthOfField>();
        m_SSAO = Camera.main.GetComponent<ScreenSpaceAmbientOcclusion>();
        m_SunShafts = Camera.main.GetComponent<SunShafts>();
        m_CCC = Camera.main.GetComponent<ColorCorrectionCurves>();
    }

    void Update () {

        m_Bloom.enabled = (BloomTog.isOn)         ? true : false;
        m_Depth.enabled = (DofTog.isOn)           ? true : false;
        m_SSAO.enabled = (AmbientOccTog.isOn)     ? true : false;
        m_SunShafts.enabled = (SunShaftsTog.isOn) ? true : false;
        m_CCC.enabled = (CcTog.isOn)              ? true : false;
    }
}
