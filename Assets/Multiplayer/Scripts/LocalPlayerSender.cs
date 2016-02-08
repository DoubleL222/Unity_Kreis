using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[NetworkSettings(channel = 1, sendInterval = 0.0625f)]
public class LocalPlayerSender : NetworkBehaviour {
	public Text DebugText;
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
			if(DebugText == null)
				DebugText = GameObject.FindGameObjectWithTag ("DebugText").GetComponent<Text>() as Text;
			DebugText.text = "my PlayerID was set to "+ playerID + "\n"+DebugText.text;
		}
	}

	void Start(){

	}
	// Use this for initialization
	[Command]
	public void CmdSendInputToServer(string input){
		if (!hasAuthority) {
			Debug.Log("NO AUTHORITY");
		}
		if (isServer) {

			DebugText.text = "Received Command "+input+ " from Client with id: "+PlayerID+	 "\n"+DebugText.text;
		}
	}
}
