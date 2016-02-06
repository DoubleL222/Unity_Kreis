using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NetworkMessenger : NetworkBehaviour {

	public Text DebugText;
	// Use this for initialization
	NetworkClient m_client;
	const short MyBeginMsg = 1002;
	// Update is called once per frame
	void Start () {
		NetworkClient nc = new NetworkClient ();
	}

	void Update () {
	
	}

	public void SendReadyToBeginMessage()
	{
		var msg = new StringMessage("Some text MESSAGE");
		m_client.Send(MyBeginMsg, msg);
	}

	public void Init(NetworkClient client)
	{
		m_client = client;
		NetworkServer.RegisterHandler(MyBeginMsg, OnServerReadyToBeginMessage);
	}
	
	void OnServerReadyToBeginMessage(NetworkMessage netMsg)
	{
		var beginMessage = netMsg.ReadMessage<StringMessage>();
		DebugText.text = "received OnServerReadyToBeginMessage " + beginMessage.value + "\n" + DebugText.text;
		//Debug.Log("received OnServerReadyToBeginMessage " + beginMessage.value);
	}
}
