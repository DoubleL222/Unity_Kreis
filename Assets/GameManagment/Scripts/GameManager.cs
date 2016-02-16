﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// A class that will oversee the whole game
/// </summary>
public class GameManager : MonoBehaviour
{
	public GameObject root;

	public static GameManager GMInstance;

	List<Color> playerColors;
	
	List<RingManager> rings;

	public int NumPlayers = 2;
	public int winScore = 10;
	bool scoreReached=false;

	public static bool gameEnded = true;
	private bool toStart = true;

	public Sprite barrier, indestructableBarrier;

	List<GameObject> LivingPlayers;

	string[] playerNames;
	float[] RingSizes;
	Vector3[] SpawnPositions;
	List<SegmentCollisionBehaviour>[] segmentCollisionBehaviours;
	List<SegmentTriggerBehaviour>[] segmentTriggerBehaviours;
	List<SegmentTickBehaviour>[] segmentTickBehaviours;
	Sprite[] segmentSprites;
	static int[] playerScores;

	float PhaseInDelay = float.MaxValue;

	public GameObject PhaseInEffect;
	//public GameObject localPlayerPrefab;
    public GameObject[] localPlayerPrefabs;
    private int[] prefabInd;
	public WinnerCanvasController WCC;
    public lobbyScript lobby;

	public Text[] playerTexts;

	//MULTIPLAYER
	public bool multiplayerMode = false;
	public List<LocalPlayerSender> LocalPlayerSenders = new List<LocalPlayerSender>();


	// powerups
	PowerUpSpawner PUS;
	public bool usePowerUps;
	[HideInInspector]
	public List<PowerUpManager> activePowerUps = new List<PowerUpManager>();

	/// <summary>
	/// Starts the game. Initializes rings, players, sounds, ...
	/// </summary>
	public void addLCP (LocalPlayerSender LPS)
	{
		LocalPlayerSenders.Add (LPS);

	}
	public void StartGame(){
        //lobbyCanvas.enabled = false;

        string[] names = lobby.getNames();
		winScore = lobby.getWinScore ();
		scoreReached = false;
        for(int i = 0; i < names.Length; i++)
        {
            if (names[i] != "")
            {
                playerNames[i] = names[i];
            }
            else
            {
                playerNames[i] = "player " + (i + 1);
            }
        }
        prefabInd = lobby.getPlayerColorInd();
        NumPlayers = lobby.getNumOfPlayers();
        lobby.hideLobby();


		SpawnRings ("basic");
		SpawnPlayers (NumPlayers);
		SoundManager.PlayBigBoomClip ();

		// powerups
    
		if (usePowerUps) {
			PUS = gameObject.GetComponent<PowerUpSpawner> ();
		}
	}

	void Awake ()
	{
		GMInstance = this;
	}

	void Start ()
	{
		playerScores = new int[NumPlayers];
		LivingPlayers = new List<GameObject> ();
		playerColors = new List<Color> ();
		playerColors.Add (new Color32 (255, 238, 13, 255));
		playerColors.Add (new Color32 (232, 94, 12, 255));
		playerColors.Add (new Color32 (234, 0, 255, 255));
		playerColors.Add (new Color32 (12, 99, 232, 255));
		playerColors.Add (new Color32 (0, 255, 69, 255));
		for (int i = 0; i < playerTexts.Length; i++) {
			playerTexts [i].color = playerColors [i];
			playerTexts [i].enabled = false;
		}
			
		ParticleSystem[] PSS = PhaseInEffect.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem PS in PSS) {
			if (PS.duration < PhaseInDelay) {
				PhaseInDelay = PS.duration;
			}
		}
		if (PhaseInDelay == float.MaxValue) {
			PhaseInDelay = 5.0f;
		}
		playerNames = new string[10];
		playerNames [0] = "Phil";
		playerNames [1] = "Amy";
		playerNames [2] = "Carl";
		playerNames [3] = "Lucy";
		playerNames [4] = "Nick";
		playerNames [5] = "Megan";
		playerNames [6] = "Ralph";
		playerNames [7] = "Don";
		playerNames [8] = "Liz";
		playerNames [9] = "Bob";

		int nrings = 3;

		RingSizes = new float[nrings];
		RingSizes [0] = 10f;
		RingSizes [1] = 17f;
		RingSizes [2] = 24f;

		segmentTickBehaviours = new List<SegmentTickBehaviour>[nrings];
		for (int i = 0; i < segmentTickBehaviours.Length; i++)
			segmentTickBehaviours [i] = new List<SegmentTickBehaviour> ();
		segmentTickBehaviours [0].Add (new SegmentTickBehaviourMove (5f));
		segmentTickBehaviours [1].Add (new SegmentTickBehaviourMove (-3f));
		segmentTickBehaviours [2].Add (new SegmentTickBehaviourMove (4f));

		segmentCollisionBehaviours = new List<SegmentCollisionBehaviour>[nrings];
		for (int i = 0; i < segmentCollisionBehaviours.Length; i++)
			segmentCollisionBehaviours [i] = new List<SegmentCollisionBehaviour> ();

		segmentTriggerBehaviours = new List<SegmentTriggerBehaviour>[nrings];
		for (int i = 0; i < segmentTriggerBehaviours.Length; i++)
			segmentTriggerBehaviours [i] = new List<SegmentTriggerBehaviour> ();
		
		segmentTriggerBehaviours [1].Add (new SegmentTriggerBehaviourDestroy ());

		segmentSprites = new Sprite[nrings];
		segmentSprites [0] = indestructableBarrier;
		segmentSprites [1] = barrier;
		segmentSprites [2] = indestructableBarrier;
	}

	// Update is called once per frame

	void Update ()
	{
		if (toStart) {
			StartGame ();
			toStart = false;
		}
	}

	/// <summary>
	/// Spawns the rings defined in RingSizes.
	/// </summary>
	/// <param name="RingSizes">A 2d array of [distance, speed]</param>
	public void SpawnRings (string arenaname)
	{
		if (rings != null) {
			for (int i = 0; i < rings.Count; i++) {
				Destroy (rings [i].SegmentsParent);
			}
		}
		rings = new List<RingManager> ();
		ArenaData ad = ArenaDataLoader.arenas [arenaname];

		for (int i = 0; i < ad.rings.Count; i++) {
			rings.Add (new RingManager (ad.rings[i].size, ad.rings[i].segmentCollisionBehaviours, ad.rings[i].segmentTickBehaviours, ad.rings[i].segmentTriggerBehaviours, ad.rings[i].sprite));
		}
	}

	/// <summary>
	/// Spawns the players.
	/// </summary>
	/// <param name="NumberOfPlayers">Number of players.</param>
	public void SpawnPlayers (int NumberOfPlayers)
	{
		LivingPlayers.Clear ();

		gameEnded = false;
		SpawnPositions = CalculateSpawnPositions (NumberOfPlayers);
        /*IDictionary<string,KeyCode> p1keys = new Dictionary<string,KeyCode> ();
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
		p4keys.Add ("shoot", KeyCode.Keypad5);*/

        IDictionary<string, string> p1keys = new Dictionary<string, string>();
        p1keys.Add("movement", "P1Movement");
        p1keys.Add("gravityChange", "P1Jump");
        p1keys.Add("shoot", "P1Shoot");

        IDictionary<string, string> p2keys = new Dictionary<string, string>();
        p2keys.Add("movement", "P2Movement");
        p2keys.Add("gravityChange", "P2Jump");
        p2keys.Add("shoot", "P2Shoot");

        IDictionary<string, string> p3keys = new Dictionary<string, string>();
        p3keys.Add("movement", "P3Movement");
        p3keys.Add("gravityChange", "P3Jump");
        p3keys.Add("shoot", "P3Shoot");

        IDictionary<string, string> p4keys = new Dictionary<string, string>();
        p4keys.Add("movement", "P4Movement");
        p4keys.Add("gravityChange", "P4Jump");
        p4keys.Add("shoot", "P4Shoot");


        IDictionary<string,string>[] keyCodes = new IDictionary<string, string>[10];
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

		Debug.Log ("called Spawn Player");
		//StartCoroutine( SpawnPlayerAfter (5.0f));
		Vector3 SpawnPosition = new Vector3 (0, 17f, 0);

		for (int i = 0; i < NumberOfPlayers; i++) {
			//StartCoroutine (SpawnPlayerAfter (keyCodes [i], SpawnPositions [i], i));
			StartCoroutine(SpawnPlayerAfter(keyCodes[i], ArenaDataLoader.arenas["basic"].getSpawnPosition(NumberOfPlayers, i), prefabInd[i] ,playerNames[i]));
		}
	}

	void MultiSpawnPlayers ()
	{
		SpawnPositions = CalculateSpawnPositions (LocalPlayerSenders.Count);
		Debug.Log ("NUMBEr OF LOCAL PLAYER SENDErS:" + LocalPlayerSenders.Count);
		int i = 0;
		foreach (LocalPlayerSender LPS in LocalPlayerSenders) {
			StartCoroutine (MultiSpawnPlayerAfter (SpawnPositions [i], i, LPS));
			i++;
		}
	}

	/// <summary>
	/// Calculates the spawn positions.
	/// </summary>
	/// <returns>The spawn positions.</returns>
	/// <param name="NumPlayers">Number of players.</param>
	Vector3[] CalculateSpawnPositions (int NumPlayers)
	{
		Vector3[] positons = new Vector3[NumPlayers];
		for (int i = 0; i < NumPlayers; i++) {
			int x = 0;
			if (i + 1 >= RingSizes.GetLength (0))
				x = 1;
			if (i + 1 >= RingSizes.GetLength (0) * 2 - 1)
				x = 2;
			if (i + 1 >= RingSizes.GetLength (0) * 3 - 2)
				x = 3;
			if (i + 1 >= RingSizes.GetLength (0) * 4 - 3)
				x = 4;
			if (i + 1 >= RingSizes.GetLength (0) * 5 - 4)
				x = 5;
			int j = i % (RingSizes.GetLength (0) - 1);
			positons [i] = new Vector3 (x * 30.0f, (RingSizes [j + 1] + RingSizes [j]) / 2.0f, 0.0f);
		}
		return positons;
	}

	public void PlayerDied (GameObject player)
	{
		Debug.Log ("playerDied");
		if (!gameEnded) {
			Debug.Log ("pLivingPlayers.count = " + LivingPlayers.Count);
			LivingPlayers.Remove (player);
			Debug.Log ("LivingPlayers.count = " + LivingPlayers.Count);
			for (int i = 0; i < LivingPlayers.Count; i++) {
				GameObject go = LivingPlayers [i];
				LocalPlayerController LCP = go.GetComponent<LocalPlayerController> ();
				playerScores [LCP.playerIndex]++;
				Debug.Log ("Player " + LCP.PlayerName + " (" + LCP.playerIndex + ") score " + playerScores [LCP.playerIndex]);
				playerTexts [LCP.playerIndex].text = LCP.PlayerName + ": " + playerScores [LCP.playerIndex];
			}
			bool sameTeam = true;
			int team = LivingPlayers[0].GetComponent<LocalPlayerController>().team;
			for (int i=1; i < LivingPlayers.Count; i++)
			{
				if (LivingPlayers[i].GetComponent<LocalPlayerController>().team != team)
				{
					sameTeam = false;
				}
			}
			if (sameTeam) {
				EndGame ();
			}
		}
	}


	IEnumerator SpawnPlayerAfter (IDictionary<string,string> playerKeys, Vector2 SpawnPos, int playerI, string name)
	{
		Vector3 SpawnPosition = new Vector3 (SpawnPos.x, SpawnPos.y, 0);
		GameObject pie = Instantiate (PhaseInEffect, UtilityScript.transformToCartesian (SpawnPosition), Quaternion.identity) as GameObject;
		pie.transform.SetParent (GameManager.GMInstance.transform);
		yield return new WaitForSeconds (PhaseInDelay);
		SoundManager.PlaySpawnClip ();
		GameObject localPlayer = MonoBehaviour.Instantiate (localPlayerPrefabs [(playerI % (localPlayerPrefabs.Length))], SpawnPosition, new Quaternion ()) as GameObject;
		localPlayer.transform.SetParent (GameManager.GMInstance.root.transform);
		LivingPlayers.Add (localPlayer);
		//SpriteRenderer PlayerSR = localPlayer.GetComponentInChildren<SpriteRenderer> ();
		//Debug.Log ("GETTING COLOR AT INDEX " + playerI);
		//PlayerSR.color = playerColors[(playerI % (playerColors.Count))];

		LocalPlayerController LCP = localPlayer.GetComponent<LocalPlayerController> ();
		LCP.team = playerI;
		LCP.playerIndex = playerI;
		LCP.setKeys (playerKeys);
		LCP.PlayerName = name;


		playerTexts [playerI%4].text = LCP.PlayerName + ": " + playerScores [playerI%4];
		playerTexts [playerI%4].enabled = true;

		cameraLoc.updatePlayers = true;

		if (usePowerUps)
			PUS.spawnPowerups = true;
	}

	IEnumerator MultiSpawnPlayerAfter (Vector3 SpawnPosition, int playerI, LocalPlayerSender LPS)
	{
		GameObject pie = Instantiate (PhaseInEffect, UtilityScript.transformToCartesian (SpawnPosition), Quaternion.identity) as GameObject;
		pie.transform.SetParent (GameManager.GMInstance.root.transform);
		yield return new WaitForSeconds (PhaseInDelay);
		SoundManager.PlaySpawnClip ();
		GameObject localPlayer = MonoBehaviour.Instantiate (localPlayerPrefabs [(playerI % (localPlayerPrefabs.Length))], SpawnPosition, new Quaternion ()) as GameObject;
		localPlayer.transform.SetParent (GameManager.GMInstance.root.transform);
		LivingPlayers.Add (localPlayer);
		//SpriteRenderer PlayerSR = localPlayer.GetComponentInChildren<SpriteRenderer> ();
		//Debug.Log ("GETTING COLOR AT INDEX " + playerI);
		//PlayerSR.color = playerColors[(playerI % (playerColors.Count))];
		LocalPlayerController LCP = localPlayer.GetComponent<LocalPlayerController> ();
		LCP.PlayerName = playerNames [playerI % playerNames.Length];
		LPS.myPlayerOnTheServer = LCP;
		LCP.playerIndex = playerI;

		playerTexts [playerI].text = LCP.PlayerName + ": " + playerScores [playerI];
		playerTexts [playerI].enabled = true;

		cameraLoc.updatePlayers = true;
		if (usePowerUps)
			PUS.spawnPowerups = true;
	}
	
	public void FinalDestruction (float delayStep)
	{
		PUS.enabled = false;
		
		float mem = delayStep;
		float maxDelay = .0f;

		foreach (RingManager rm in rings) {
			//Debug.Log ("final destruction called, segment controllers : " + rm.segmentControlers.Count);
			delayStep = mem;
			foreach (SegmentController sc in rm.segmentControlers) {
				StartCoroutine (sc.DestroySegmentAFter (delayStep / rm.segmentControlers.Count));
				delayStep += mem;
				if ((delayStep / rm.segmentControlers.Count) > maxDelay)
					maxDelay = delayStep / rm.segmentControlers.Count;
			}
		}
		foreach (PowerUpManager PUM in activePowerUps)
		{
			StartCoroutine(PUM.DestroyPowerUpAfter(maxDelay));
		}
	}
	
	void EndGame ()
	{	

		gameEnded = true;
		FinalDestruction (3.5f);
		LocalPlayerController LPC = LivingPlayers[0].GetComponent<LocalPlayerController> ();

		for (int i=0; i < playerScores.Length; i++) {
			if(playerScores[i] >= winScore){
				scoreReached=true;
			}
		}

		if (scoreReached) {

			//TODO: end round
			string PName = "";
			if (LivingPlayers.Count == 1)
			{
				PName = LPC.PlayerName+" WINS!";
			}
			else
			{
				for(int i = 0; i < LivingPlayers.Count; i++)
				{
					PName+=LivingPlayers[i].GetComponent<LocalPlayerController>().PlayerName+" ";
				}
				PName += "WIN!";
			}

			Destroy (LPC.gameObject);
			cameraLoc.updatePlayers=true;
			GameObject playerMesh = Instantiate (LPC.mesh, LPC.mesh.transform.position, LPC.mesh.transform.rotation) as GameObject;
			playerMesh.transform.SetParent(GMInstance.root.transform);
			WCC.FinishGame (playerMesh, PName);
			StartCoroutine(backToLobby());
		} else {
			Destroy (LPC.gameObject);
			StartCoroutine (nextRound());
		}

	}

	IEnumerator nextRound(){
		yield return new WaitForSeconds (5.5f);
		Main.getInstance ().nextRound ();
	}

	IEnumerator backToLobby(){
		yield return new WaitForSeconds (8f);
		Main.getInstance ().backToLobby ();
	}

	public void ClickServerStartGame ()
	{
		SpawnRings ("basic");
		MultiSpawnPlayers ();
		SoundManager.PlayBigBoomClip ();
	}
}
