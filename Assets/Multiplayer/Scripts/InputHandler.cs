using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
	public Text DebugText;
	private LocalPlayerSender LPC;

	void Start(){
		DebugText = GameObject.FindGameObjectWithTag ("DebugText").GetComponent<Text>() as Text;
		SetupInputHandler ();
		FindMyButtons ();
	}
	// Use this for initialization
	public void MoveShipRightButtonClick(){
		if (LPC == null) {
			DebugText.text = "LPC for this client not set"+ "\n"+DebugText.text;
		}
		LPC.CmdSendInputToServer ("Move ship Right button clicked");
	}
	
	void SetupInputHandler(){
		LPC = GetComponent<LocalPlayerSender> () as LocalPlayerSender;
	}
	void FindMyButtons(){
		GameObject.FindGameObjectWithTag ("ButtonRight").GetComponent<Button> ().onClick.AddListener (() => {
			MoveShipRightButtonClick();});
	}
	// Update is called once per frame
	void Update () {
	
	}
}
