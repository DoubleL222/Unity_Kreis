using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	List<RingManager> rings;

	GameObject localPlayer1;
	GameObject localPlayer2;

	// Use this for initialization
	void Start() {
		GameObject localPlayerPrefab = Resources.Load ("_new/LocalPlayer") as GameObject;
		localPlayer1 = MonoBehaviour.Instantiate (localPlayerPrefab, new Vector3 (0, 17.5f, 0), new Quaternion ()) as GameObject;
		localPlayer2 = MonoBehaviour.Instantiate (localPlayerPrefab, new Vector3 (0, 17.5f, 0), new Quaternion ()) as GameObject;

		IDictionary<string,string> p2keys = new Dictionary<string,string> ();
		p2keys.Add ("left", "g");
		p2keys.Add ("right", "j");
		p2keys.Add ("gravityChange", "z");
		p2keys.Add ("shoot", "h");
		localPlayer2.GetComponent<LocalPlayerController>().setKeys (p2keys);


		rings = new List<RingManager> ();
		rings.Add (new RingManager (25f));
		rings.Add (new RingManager (20f));
		rings.Add (new RingManager (15f));
		rings.Add (new RingManager (10f));
		//rings.Add (new RingManager (5f));
		//Debug.Log ("Initialized!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
