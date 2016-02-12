using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NetworkMessenger : MonoBehaviour {

	public Text DebugText;
	// Use this for initialization
	const short MyBeginMsg = 1002;
	// Update is called once per frame
	NetworkIdentity NI;
	NetworkConnection NC;

	NetworkClient ThisClient;

	public void init(NetworkConnection nc){
		if (nc != null) {
			NC = nc;
			IntegerMessage newMSG = new IntegerMessage(1);
			nc.Send (MyBeginMsg, newMSG);
			//NetworkClient newNetworkClient = new NetworkClient();
			//newNetworkClient.RegisterHandler();
			DebugText.text = "Got NetworkConnectionFromSerever \n" + DebugText.text;
		}
		NI = gameObject.GetComponent<NetworkIdentity> ();
		if (NI != null) {
			DebugText.text = "Found network identity on client \n" + DebugText.text;
		}
	}

	public void SendMessageToServer(){
		//if(NI.isClient){
		IntegerMessage newMSG = new IntegerMessage(1337);
		NetworkServer.SendToAll (MyBeginMsg, newMSG);
		if (NC == null) {
			DebugText.text = "Conncetion is null somehow \n" + DebugText.text; 

		} else {
			NC.Send (MyBeginMsg, newMSG);
			DebugText.text = "Sending Message To server from CLIENT \n" + DebugText.text; 
		}
		//}
	}
	public void ReceiveMessage(){

	}

}
