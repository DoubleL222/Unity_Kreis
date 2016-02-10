using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[NetworkSettings(channel = 1, sendInterval = 0.0625f)]
public class LocalPlayerSender : NetworkBehaviour {
	public LocalPlayerController myPlayerOnTheServer;
	private Text DebugText;
	public GameObject PlayerControlerPrefab;
	private int playerID;
	public int PlayerID
	{
		get
		{
			//Some other code
			return playerID;
		}
		set
		{
			//Some other code
			playerID = value;
			//if(DebugText == null)
				//DebugText = GameObject.FindGameObjectWithTag ("DebugText").GetComponent<Text>() as Text;
			//DebugText.text = "my PlayerID was set to "+ playerID + "\n"+DebugText.text;
		}
	}

	void Start(){
		if (isLocalPlayer) {
			GameObject go =	Instantiate (PlayerControlerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			InputHandler myInputHandler = GetComponent<InputHandler>();
			go.GetComponentInChildren<ButtonLeftGetter>().inputHandler = myInputHandler;
			go.GetComponentInChildren<ButtonRightGetter>().inputHandler = myInputHandler;
			go.GetComponentInChildren<ButtonFireGetter>().inputHandler = myInputHandler;
			go.GetComponentInChildren<ButtenUpGetter>().inputHandler = myInputHandler;
			/*ButtonInputGetter[] BIGs = go.GetComponentsInChildren<ButtonInputGetter>();
			InputHandler myInputHandler = GetComponent<InputHandler>();
			foreach(ButtonInputGetter BIG in BIGs){
				BIG.inputHandler = myInputHandler;
			}*/
		}
	}
	/*
	// Use this for initialization
	[Command]
	public void CmdLeftState(bool state)
	{
		if (myPlayerOnTheServer != null && isServer)
			//myPlayerOnTheServer.leftPressed = state;
	}

	[Command]
	public void CmdRightState(bool state)
	{
		if (myPlayerOnTheServer != null && isServer)
			//myPlayerOnTheServer.rightPressed = state;
	}

	[Command]
	public void CmdFireState(bool state)
	{
		if (myPlayerOnTheServer != null && isServer)
			//myPlayerOnTheServer.firePressed = state;
	}

	[Command]
	public void CmdJumpState(bool state)
	{
		if (myPlayerOnTheServer != null && isServer)
			//myPlayerOnTheServer.jumpPressed = state;
	}
	*/
	[Command]
	public void CmdPlayerPressingButton(bool leftPressed, bool rightPressed, bool jumpPressed, bool firePressed)
	{
		if (myPlayerOnTheServer != null)
		{
			if (isServer)
			{
				myPlayerOnTheServer.leftPressed = leftPressed;
				myPlayerOnTheServer.rightPressed = rightPressed;
				myPlayerOnTheServer.jumpPressed = jumpPressed;
				myPlayerOnTheServer.firePressed = firePressed;
				/*
				//DebugText.text = "Player with ID: " + PlayerID + " IS PRESSING BUTTON: " + pInput.ToString () + "\n" + DebugText.text;
				switch (pInput) {
				case PlayerInput.left:
					myPlayerOnTheServer.PlayerLeftControll ();
					break;
				case PlayerInput.right:
					myPlayerOnTheServer.PlayerRightControll ();
					break;
				case PlayerInput.shoot:
					myPlayerOnTheServer.PlayerShootControll ();
					break;
				case PlayerInput.up:
					myPlayerOnTheServer.PlayerGravityShiftControll ();
					break;
				default:
					break;
				}
				*/
			}
		}
	}
	[Command]
	public void CmdPlayerLetGoButton(PlayerInput pInput)
	 {

	}
}
