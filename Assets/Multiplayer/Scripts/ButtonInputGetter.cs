using UnityEngine;
using System.Collections;

public class ButtonInputGetter : MonoBehaviour {
	PlayerInput thisButton;
	public InputHandler inputHandler;
	// Use this for initialization
	void Start () {
		switch (gameObject.tag) {
		case "buttonLeft":
			thisButton = PlayerInput.left;
			break;
		case "buttonRight":
			thisButton = PlayerInput.right;
			break;
		case "buttonUp":
			thisButton = PlayerInput.up;
			break;
		case "buttonShoot":
			thisButton = PlayerInput.shoot;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown(){
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.ButtonPressedDown (thisButton);
	}
	void OnMouseUp(){
		//Debug.Log ("Let go of button" + thisButton.ToString());
		inputHandler.ButtonLetGo (thisButton);
	}
}
