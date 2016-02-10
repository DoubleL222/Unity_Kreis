using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public enum PlayerInput
{
	left, right, up, shoot
};

public class InputHandler : MonoBehaviour {


	private Text DebugText;
	private LocalPlayerSender LPC;
	private List<PlayerInput> currentlyPressedButtons;

	public bool firePressed = false;
	public bool leftPressed = false;
	public bool rightPressed = false;
	public bool jumpPressed = false;
	public bool inputChanged = false;

	void Start(){
		currentlyPressedButtons = new List<PlayerInput>();
		DebugText = GameObject.FindGameObjectWithTag ("DebugText").GetComponent<Text>() as Text;
		SetupInputHandler ();
	}
	// Use this for initialization
	
	void SetupInputHandler(){
		LPC = GetComponent<LocalPlayerSender> () as LocalPlayerSender;
	}

	public void ButtonPressedDown(PlayerInput butt){
		/*if (!currentlyPressedButtons.Contains(butt)) {
			currentlyPressedButtons.Add(butt);
		}*/
		switch (butt) {
		case PlayerInput.left:
			leftPressed = true;
			break;
		case PlayerInput.right:
			rightPressed = true;
			break;
		case PlayerInput.up:
			jumpPressed = true;
			break;
		case PlayerInput.shoot:
			firePressed = true;
			break;
		default:
			break;
		}
	}
	public void ButtonLetGo(PlayerInput butt){
		/*if (currentlyPressedButtons.Contains (butt)) {
			currentlyPressedButtons.Remove(butt);
			//LPC.CmdPlayerLetGoButton(butt);
		}*/
		switch (butt) {
		case PlayerInput.left:
			leftPressed = false;
			break;
		case PlayerInput.right:
			rightPressed = false;
			break;
		case PlayerInput.up:
			jumpPressed = false;
			break;
		case PlayerInput.shoot:
			firePressed = false;
			break;
		default:
			break;
		}
	}
	/*
	public void PressingLeft(){
		LPC.CmdPlayerPressingButton (PlayerInput.left);
	}
	public void PressingRight(){
		LPC.CmdPlayerPressingButton (PlayerInput.right);
	}
	public void PressingUp(){
		LPC.CmdPlayerPressingButton (PlayerInput.up);
	}
	public void PressingShoot(){
		LPC.CmdPlayerPressingButton (PlayerInput.shoot);
	}
	*/
	// Update is called once per frame
	void Update ()
	{
		if (inputChanged) {
			LPC.CmdPlayerPressingButton (leftPressed, rightPressed, jumpPressed, firePressed);
			inputChanged = false;
		}

		/*
		//foreach (PlayerInput PressedButton in currentlyPressedButtons) {
		//	LPC.CmdPlayerPressingButton (PressedButton);
		//}
		if (leftPressed) {
			LPC.CmdPlayerPressingButton (PlayerInput.left);
		} else if (rightPressed) {
			LPC.CmdPlayerPressingButton (PlayerInput.right);
		}
		if (firePressed) {
			LPC.CmdPlayerPressingButton (PlayerInput.shoot);
		}
		if (jumpPressed) {
			LPC.CmdPlayerPressingButton (PlayerInput.up);
		}
		*/
	}
}
