using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinnerCanvasController : MonoBehaviour {

	public Text WinnerText;
	public Canvas finishCanvas;
	public RectTransform PanelTransform;
	Vector3 targetScale = Vector3.one;
	Vector3 targetPlayerScale = new Vector3 (15, 4.5f, 0);
	Vector3 targetPlayerPosition = new Vector3 (0, 0, -6);

	private Vector3 velocity = Vector3.zero;
	float smoothTimePanel = 0.3f;
	float smoothTimePlayer = 0.3f;

	bool animatingCanvas = false;
	bool animatingPlayerPos = false;
	bool animatingPlayerScale = false;

	GameObject winner;

	public void FinishGame(GameObject winnerPlayer, string WinnerName){
		WinnerText.text = WinnerName.ToUpper() + " WINS!";
		AnimateCanvas ();
		AnimatePlayer (winnerPlayer);
	}

	void AnimatePlayer(GameObject winnerPlayer){
		winnerPlayer.transform.rotation = Quaternion.identity;
		winner = winnerPlayer;
		animatingPlayerPos = true;
		animatingPlayerScale = true;
	}

	void AnimateCanvas(){
		finishCanvas.enabled = true;
		PanelTransform.localScale = Vector3.zero;
		animatingCanvas = true;
	}
	// Use this for initialization
	void Awake () {
		finishCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (animatingCanvas) {
			PanelTransform.localScale = Vector3.SmoothDamp (PanelTransform.localScale, targetScale, ref velocity, smoothTimePanel);
			if (PanelTransform.localScale == targetScale) {
				animatingCanvas = false;
				Debug.Log ("finished animating panel");
			}
		}
		else if (animatingPlayerScale) {
			
			winner.transform.localScale = Vector3.SmoothDamp (winner.transform.localScale, targetPlayerScale, ref velocity, smoothTimePlayer);
			if (winner.transform.localScale == targetPlayerScale) {
				animatingPlayerScale = false;
			}
		}
		else if (animatingPlayerPos) {
			winner.transform.position = Vector3.SmoothDamp (winner.transform.position, targetPlayerPosition, ref velocity, smoothTimePlayer);
			if (winner.transform.position == targetPlayerPosition) {
				animatingPlayerPos = false;
			}
		}
	}
}
