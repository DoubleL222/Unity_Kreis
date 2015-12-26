using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	List<RingManager> rings;

	GameObject localPlayer;

	// Use this for initialization
	void Start () {
		GameObject localPlayerPrefab = Resources.Load ("_new/LocalPlayer") as GameObject;
		localPlayer = MonoBehaviour.Instantiate (localPlayerPrefab, new Vector3 (0, 27.5f, 0), new Quaternion ()) as GameObject;
		rings = new List<RingManager> ();
		rings.Add (new RingManager (25f));
		rings.Add (new RingManager (30f));
		rings.Add (new RingManager (35f));
		rings.Add (new RingManager (40f));
		rings.Add (new RingManager (50f));
		Debug.Log ("Initialized!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
