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
		if (!currentlyPressedButtons.Contains(butt)) {
			currentlyPressedButtons.Add(butt);
		}
	}
	public void ButtonLetGo(PlayerInput butt){
		if (currentlyPressedButtons.Contains (butt)) {
			currentlyPressedButtons.Remove(butt);
			LPC.CmdPlayerLetGoButton(butt);
		}
	}
	// Update is called once per frame
	void Update () {
		foreach (PlayerInput PressedButton in currentlyPressedButtons) {
			LPC.CmdPlayerPressingButton(PressedButton);
		}
	}
}
