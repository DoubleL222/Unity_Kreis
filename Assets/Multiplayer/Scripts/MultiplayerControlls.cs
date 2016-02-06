using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MultiplayerControlls : NetworkBehaviour {

	// Use this for initialization
	public List<ControllCommands> getPlayerInput(IDictionary<string,KeyCode> keys){
		List<ControllCommands> cList = new List<ControllCommands> ();
		if (isLocalPlayer) {

			if (Input.GetKey (keys ["right"])) {
				cList.Add (ControllCommands.right);
			}
			if (Input.GetKey (keys ["left"])) {
				cList.Add (ControllCommands.left);
			}
			if (Input.GetKey (keys ["shoot"])) {
				cList.Add (ControllCommands.shoot);
			}
			if (Input.GetKey (keys ["gravityChange"])) {
				cList.Add (ControllCommands.gravity);
			}
		}
		return cList;
	}
}
