using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class ButtonInfo {
	public string Name;
	public GameObject ButtonObject;
	public GameObject[] Stars;
}

public class ButtonLevelLogic : MonoBehaviour {

	public GameObject WarningPanel;
	public ButtonInfo[] Buttons;
	private Color BlackedOutStarColor;

	private Image[,] m_Stars;

	void Start() {
		BlackedOutStarColor = new Color(0.125f, 0.125f, 0.125f, 1.0f);
		SetUp();

	}

	void SetUp() {
		m_Stars = new Image[Buttons.Length, 3];

		for(int button = 0; button < Buttons.Length; button++) {
			for(int star = 0; star < 3; star++) {
				m_Stars[button, star] = Buttons[button].Stars[star].GetComponent<Image>();
			}
			if(button != 0) {
				bool result = LevelStatManager.Instance.levelStats[button - 1].wasCompleted;
				Buttons[button].ButtonObject.GetComponent<Button>().interactable = result;
				if(LevelStatManager.Instance.levelStats[button].wasCompleted)
					ColorStars(button);
			} else {
				ColorStars(button);
			}
		}
	}

	void ColorStars(int p_index) {
		int starRating = LevelStatManager.Instance.levelStats[p_index].starRating;
		for(int i = 0; i < 3; i++) {
			if(i < starRating) {
				m_Stars[p_index, i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			} else {
				m_Stars[p_index, i].color = BlackedOutStarColor;
			}
		}
	}

	public void NewGameReset() {
		SaveLoad temp = FindObjectOfType<SaveLoad>();
		temp.ResetSave();
		
	}

	public void PopUpWarning() {
		WarningPanel.SetActive(true);
	}

	public void ClosePanel() {
		WarningPanel.SetActive(false);
	}
}
