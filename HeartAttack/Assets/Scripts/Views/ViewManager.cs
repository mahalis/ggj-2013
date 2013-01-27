using UnityEngine;
using System.Collections;

public class ViewManager : MonoBehaviour {
	public GameObject centerAnchor;
	public GameObject upperRightAnchor;
	public GameObject upperLeftAnchor;

	public GameObject startGameView;
	public GameObject gameOverView;
	public TextMesh scoreMesh;

	public GUIText heartBPMGUI;

	void Start() {
		setGameOverViewVisible(false);
		this.positionAnchors();
	}

	void positionAnchors() {
		if (centerAnchor) {
			centerAnchor.transform.localPosition = Vector3.zero;
		}
		if (upperRightAnchor) {
			upperRightAnchor.transform.localPosition = new Vector3(Screen.width, Screen.height,0);
		}
		if (upperLeftAnchor) {
			upperLeftAnchor.transform.localPosition = new Vector3(-Screen.width, Screen.height,0);
		}
	}

	public void hideStartGameView() {
		startGameView.SetActive(false);
	}

	public void setGameOverViewVisible(bool visible) {
		gameOverView.SetActive(visible);
	}

	public void setScore(int score) {
		if (scoreMesh){
			scoreMesh.text = score.ToString();
		}
	}

	public void setBpm(float bpm) {
		if (heartBPMGUI){
			heartBPMGUI.text = ((int)bpm).ToString() + " BPM";
		}
	}
	
}
