using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyNetworkManager : NetworkManager {
	const short MyBeginMsg = 1002;
	public Text DebuggingText;
	int playerIds = 0;

	public GameObject MessageSender;
	//public MySpawnerScript MSS;

	List<GameObject> messageSenders;

	void Start(){
		messageSenders = new List<GameObject> ();
	}
	// Use this for initialization
	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		//NetworkClient nc = new NetworkClient ();
//		conn.RegisterHandler (MyBeginMsg, HandlePlayerInput);
		//Debug.Log ("client connected");
		DebuggingText.text = "Client connected with connection id: "+ conn.connectionId +", with adress: " + conn.address + "\n"+ DebuggingText.text;
		//NetworkServer.SpawnObjects ();
	}

//	[Server]
	void spawnMessageSender(NetworkConnection conn){
		var go = (GameObject)Instantiate(MessageSender, Vector3.zero, Quaternion.identity);
		NetworkServer.SpawnWithClientAuthority(go, conn);
		//messageSenders.Add (go);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect (conn);
//		NetworkMessenger NM = FindObjectOfType<NetworkMessenger> ();
//		NM.init (conn);
		//Debug.Log ("client connected, i am client");
		//if(conn.address!="localServer")
		//spawnMessageSender (conn);
		//ClientScene.AddPlayer(0);
		//ClientScene.RegisterPrefab (MessageSender);
		//spawnMessageSender (conn);
		//GameObject.FindObjectOfType<InputHandler>().MessageSender = messageSenders[messag
	//	int newID = NetworkServer.connections.Count;
	//	FindObjectOfType<InputHandler> ().InputHandlerID = newID;

		DebuggingText.text = "I am client, and have connected to server con: " + conn.connectionId + ", with adress: " + conn.address + ", host id:" + conn.hostId + "\n" + DebuggingText.text;
	}
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		player.GetComponent<LocalPlayerSender>().PlayerID = playerIds++;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
}
