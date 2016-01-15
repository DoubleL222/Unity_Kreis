﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	List<RingManager> rings;

	GameObject localPlayer1;
	GameObject localPlayer2;

	static GameObject PhaseInEffect;
	static GameObject localPlayerPrefab;
	// Use this for initialization
	void Start() {
		if(localPlayerPrefab == null)
			localPlayerPrefab = Resources.Load ("_new/LocalPlayer") as GameObject;
		if(PhaseInEffect == null)
			PhaseInEffect = Resources.Load ("_new/PhaseInEffect") as GameObject;

		Vector3 SpawnPosition = new Vector3 (0, 15f, 0);

		Instantiate (PhaseInEffect, transformToPolar (SpawnPosition), Quaternion.identity);

		localPlayer1 = MonoBehaviour.Instantiate (localPlayerPrefab, SpawnPosition, new Quaternion ()) as GameObject;
		localPlayer2 = MonoBehaviour.Instantiate (localPlayerPrefab, SpawnPosition, new Quaternion ()) as GameObject;

		IDictionary<string,string> p2keys = new Dictionary<string,string> ();
		p2keys.Add ("left", "g");
		p2keys.Add ("right", "j");
		p2keys.Add ("gravityChange", "z");
		p2keys.Add ("shoot", "h");
		localPlayer2.GetComponent<LocalPlayerController>().setKeys (p2keys);


		rings = new List<RingManager> ();
	//	rings.Add (new RingManager (35f));
		rings.Add (new RingManager (24f));
		rings.Add (new RingManager (17f));
		rings.Add (new RingManager (10f));
		//rings.Add (new RingManager (5f));
		//Debug.Log ("Initialized!");
	}
	Vector3 transformToPolar(Vector3 pos){
		
		float angle = pos.x / 10.0f;
		float distance = pos.y;
		
		float mx = distance * Mathf.Cos (angle);
		float my = distance * Mathf.Sin (angle);
		
		return new Vector3 (mx, my, 0.0f);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
