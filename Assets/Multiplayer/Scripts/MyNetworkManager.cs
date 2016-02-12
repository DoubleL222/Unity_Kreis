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
	public GameObject PlayerObject;
	public GameObject MessageSender;
	public GameObject scriptsObjectPrefab;
	public GameObject[] SceneEffectObjects;

	private GameManager serverGameManager;
	//public MySpawnerScript MSS;
	public List<GameObject> AllPlayerObjects;

	void Start(){
	}
	// Use this for initialization
	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect (conn);
		if (serverGameManager == null) {
			SetupServerScene();
		}
		DebuggingText.text = "Client connected with connection id: "+ conn.connectionId +", with adress: " + conn.address + "\n"+ DebuggingText.text;
	}
	void SetupServerScene(){
		var go = (GameObject)Instantiate(scriptsObjectPrefab, Vector3.zero, Quaternion.identity);
		serverGameManager = go.GetComponent<GameManager> ();
		NetworkServer.Spawn (go);
		foreach (GameObject ServerSceneObject in SceneEffectObjects) {
			go = (GameObject)Instantiate(ServerSceneObject, ServerSceneObject.transform.position, Quaternion.identity);
			//serverGameManager = go.GetComponent<GameManager> ();
			NetworkServer.Spawn (go);
		}
	}

	void StartPlayingScene()
	{

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
//

		DebuggingText.text = "I am client, and have connected to server con: " + conn.connectionId + ", with adress: " + conn.address + ", host id:" + conn.hostId + "\n" + DebuggingText.text;
	}
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log ("ON SERVER ADD PLAYER CALLED");
		//GameObject go = Instantiate (PlayerObject, Vector3.zero, Quaternion.identity) as GameObject;
		GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3 (0, 20f, 0), Quaternion.identity);
		LocalPlayerSender hisLCP = player.GetComponent<LocalPlayerSender> ();
		hisLCP.PlayerID = playerIds++;
		//hisLCP.myPlayerOnTheServer = go.GetComponent<LocalPlayerController> ();
		//go.SetActive (false);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		serverGameManager.addLCP (hisLCP);
	}
}
