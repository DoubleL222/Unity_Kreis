using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager {
	public Text DebuggingText;
	// Use this for initialization
	public override void OnServerConnect(NetworkConnection conn)
	{
		//Debug.Log ("client connected");
		DebuggingText.text = "Client connected with connection id: "+ conn.connectionId +", with adress: " + conn.address + "\n"+ DebuggingText.text;
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		//Debug.Log ("client connected, i am client");
		DebuggingText.text = "I am client, and have connected to server con: " + conn.connectionId + ", with adress: " + conn.address + ", host id:" + conn.hostId + "\n" + DebuggingText.text;
	}
}
