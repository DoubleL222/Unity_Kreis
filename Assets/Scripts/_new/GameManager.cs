using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	List<RingManager> rings;

	GameObject localPlayer1;
	GameObject localPlayer2;

	float[] RingSizes;
	Vector3[] SpawnPositions;
	float PhyciscSegmentOffset = 7.0f;
	float PhaseInDelay = float.MaxValue;

	public GameObject PhaseInEffect;
	public GameObject localPlayerPrefab;
	// Use this for initialization
	void Start() {
		ParticleSystem[] PSS = PhaseInEffect.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem PS in PSS) {
			if (PS.duration < PhaseInDelay) {
				PhaseInDelay = PS.duration;
			}
		}
		RingSizes = new float[4];
		RingSizes [0] = 10f;
		RingSizes [1] = 17f;
		RingSizes [2] = 24f;
		RingSizes [3] = 31f;

		SpawnRings (RingSizes);
		SpawnPlayers (10);

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
	public void SpawnRings(float[] RingSizes){
		rings = new List<RingManager> ();
		foreach (float f in RingSizes) {
			rings.Add (new RingManager (f));
		}
	}

	public void SpawnPlayers(int NumberOfPlayers){
		SpawnPositions = CalculateSpawnPositions (NumberOfPlayers);
		IDictionary<string,KeyCode> p1keys = new Dictionary<string,KeyCode> ();
		p1keys.Add ("left", KeyCode.A);
		p1keys.Add ("right", KeyCode.D);
		p1keys.Add ("gravityChange", KeyCode.W);
		p1keys.Add ("shoot", KeyCode.S);

		IDictionary<string,KeyCode> p2keys = new Dictionary<string,KeyCode> ();
		p2keys.Add ("left", KeyCode.LeftArrow);
		p2keys.Add ("right", KeyCode.RightArrow);
		p2keys.Add ("gravityChange", KeyCode.UpArrow);
		p2keys.Add ("shoot", KeyCode.DownArrow);

		IDictionary<string,KeyCode> p3keys = new Dictionary<string,KeyCode> ();
		p3keys.Add ("left", KeyCode.J);
		p3keys.Add ("right", KeyCode.L);
		p3keys.Add ("gravityChange", KeyCode.I);
		p3keys.Add ("shoot", KeyCode.K);

		IDictionary<string,KeyCode> p4keys = new Dictionary<string,KeyCode> ();
		p4keys.Add ("left", KeyCode.Keypad4);
		p4keys.Add ("right", KeyCode.Keypad6);
		p4keys.Add ("gravityChange", KeyCode.Keypad8);
		p4keys.Add ("shoot", KeyCode.Keypad5);


		IDictionary<string,KeyCode>[] keyCodes = new IDictionary<string, KeyCode>[10];
		keyCodes [0] = p1keys;
		keyCodes [1] = p2keys;
		keyCodes [2] = p3keys;
		keyCodes [3] = p4keys;
		keyCodes [4] = p4keys;
		keyCodes [5] = p4keys;
		keyCodes [6] = p4keys;
		keyCodes [7] = p4keys;
		keyCodes [8] = p4keys;
		keyCodes [9] = p4keys;

		Debug.Log ("called Spawn PLayer");
		//StartCoroutine( SpawnPlayerAfter (5.0f));
		Vector3 SpawnPosition = new Vector3 (0, 17f, 0);
		for(int i=0; i<NumberOfPlayers;i++){
			StartCoroutine( SpawnPlayerAfter (keyCodes[i], SpawnPositions[i]));
		}
		/*


		Instantiate (PhaseInEffect, transformToPolar (SpawnPosition), Quaternion.identity);

		localPlayer1 = MonoBehaviour.Instantiate (localPlayerPrefab, SpawnPosition, new Quaternion ()) as GameObject;
		localPlayer2 = MonoBehaviour.Instantiate (localPlayerPrefab, SpawnPosition, new Quaternion ()) as GameObject;

		localPlayer2.GetComponent<LocalPlayerController>().setKeys (p2keys);
		*/
	}
	Vector3[] CalculateSpawnPositions(int NumPlayers){
		Vector3[] positons = new Vector3[NumPlayers];
		for (int i = 0; i < NumPlayers; i++) {
			int x = 0;
			if (i + 1 >= RingSizes.Length) x = 1;
			if (i + 1 >= RingSizes.Length * 2 - 1) x = 2;
			if (i + 1 >= RingSizes.Length * 3 - 2) x = 3;
			int j = i % (RingSizes.Length - 1);
			positons [i] = new Vector3 (x*10.0f, (RingSizes [j + 1] + RingSizes [j])/2.0f, 0.0f);
		}
		return positons;
	}

	IEnumerator SpawnPlayerAfter(IDictionary<string,KeyCode> playerKeys, Vector3 SpawnPosition){
		Instantiate (PhaseInEffect, transformToPolar (SpawnPosition), Quaternion.identity);
		yield return new WaitForSeconds (PhaseInDelay);
		Debug.Log ("Execute Spawn Player");
		GameObject localPlayer = MonoBehaviour.Instantiate (localPlayerPrefab, SpawnPosition, new Quaternion ()) as GameObject;
		localPlayer.GetComponent<LocalPlayerController>().setKeys (playerKeys);
	}
}
