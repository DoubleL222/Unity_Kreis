using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// A class that will oversee the whole game
/// </summary>
public class GameManager : MonoBehaviour {
	public static GameManager GMInstance;

	List<Color> playerColors;
	
	List<RingManager> rings;

	private int NumPlayers = 2;
	public static bool gameEnded = false;

	public Sprite barrier, indestructableBarrier;

	List<GameObject> LivingPlayers;
    List<int> PlayerTeams;

	string[] playerNames;
	float[] RingSizes;
	Vector3[] SpawnPositions;
	List<SegmentCollisionBehaviour> [] segmentCollisionBehaviours;
	List<SegmentTriggerBehaviour> [] segmentTriggerBehaviours;
	List<SegmentTickBehaviour> [] segmentTickBehaviours;
	Sprite[] segmentSprites;

	float PhyciscSegmentOffset = 7.0f;
	float PhaseInDelay = float.MaxValue;

	public GameObject PhaseInEffect;
	//public GameObject localPlayerPrefab;
    public GameObject[] localPlayerPrefabs;
    private int[] prefabInd;
	public WinnerCanvasController WCC;
    public lobbyScript lobby;

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
	public void addLCP(LocalPlayerSender LPS){
		LocalPlayerSenders.Add (LPS);

	}
	public void StartGame(){
        //lobbyCanvas.enabled = false;
        string[] names = lobby.getNames();
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


		SpawnRings (RingSizes);
		if(!multiplayerMode)
			SpawnPlayers (NumPlayers);
		SoundManager.PlayBigBoomClip ();

    // powerups
    
		if (usePowerUps) {
			PUS = gameObject.GetComponent<PowerUpSpawner> ();
		}
	}

	void Awake(){
		GMInstance = this;
	}

	void Start() {

		LivingPlayers = new List<GameObject> ();
        PlayerTeams = new List<int> ();
		playerColors = new List<Color> ();
		playerColors.Add (new Color32 (255, 238, 13, 255));
		playerColors.Add (new Color32 (232, 94, 12, 255));
		playerColors.Add (new Color32 (234, 0, 255, 255));
		playerColors.Add (new Color32 (12, 99, 232, 255));
		playerColors.Add (new Color32 (0, 255, 69, 255));

		ParticleSystem[] PSS = PhaseInEffect.GetComponentsInChildren<ParticleSystem>();
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
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel(Application.loadedLevel);
		}
		/*if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)){
			StartGame ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			NumPlayers = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			NumPlayers = 3;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			NumPlayers = 4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			NumPlayers = 5;
		}*/
	}
	/// <summary>
	/// Spawns the rings defined in RingSizes.
	/// </summary>
	/// <param name="RingSizes">A 2d array of [distance, speed]</param>
	public void SpawnRings(float[] RingSizes){
		rings = new List<RingManager> ();
		for (int i = 0; i < RingSizes.GetLength (0); i++) {
			rings.Add (new RingManager (RingSizes[i], segmentCollisionBehaviours[i], segmentTickBehaviours[i], segmentTriggerBehaviours[i], segmentSprites[i]));
		}
	}

	/// <summary>
	/// Spawns the players.
	/// </summary>
	/// <param name="NumberOfPlayers">Number of players.</param>
	public void SpawnPlayers(int NumberOfPlayers){
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

		Debug.Log ("called Spawn PLayer");
		//StartCoroutine( SpawnPlayerAfter (5.0f));
		Vector3 SpawnPosition = new Vector3 (0, 17f, 0);
		for(int i=0; i<NumberOfPlayers;i++){
			StartCoroutine( SpawnPlayerAfter (keyCodes[i], SpawnPositions[i], prefabInd[i], playerNames[i]));
		}
	}
	void MultiSpawnPlayers(){
		SpawnPositions = CalculateSpawnPositions (LocalPlayerSenders.Count);
		Debug.Log ( "NUMBEr OF LOCAL PLAYER SENDErS:"+ LocalPlayerSenders.Count);
		int i = 0;
		foreach (LocalPlayerSender LPS in LocalPlayerSenders) 
		{
			StartCoroutine( MultiSpawnPlayerAfter (SpawnPositions[i], i, LPS));
			i++;
		}
	}
        
	/// <summary>
	/// Calculates the spawn positions.
	/// </summary>
	/// <returns>The spawn positions.</returns>
	/// <param name="NumPlayers">Number of players.</param>
	Vector3[] CalculateSpawnPositions(int NumPlayers){
		Vector3[] positons = new Vector3[NumPlayers];
		for (int i = 0; i < NumPlayers; i++) {
			int x = 0;
			if (i + 1 >= RingSizes.GetLength (0)) x = 1;
			if (i + 1 >= RingSizes.GetLength (0) * 2 - 1) x = 2;
			if (i + 1 >= RingSizes.GetLength (0) * 3 - 2) x = 3;
			if (i + 1 >= RingSizes.GetLength (0) * 4 - 3) x = 4;
			if (i + 1 >= RingSizes.GetLength (0) * 5 - 4) x = 5;
			int j = i % (RingSizes.GetLength (0) - 1);
			positons [i] = new Vector3 (x*30.0f, (RingSizes [j + 1] + RingSizes [j])/2.0f, 0.0f);
		}
		return positons;
	}
	public void PlayerDied(GameObject player){
		Debug.Log ("playerDied");
		if (!gameEnded) {
            PlayerTeams.RemoveAt(LivingPlayers.IndexOf(player));
			LivingPlayers.Remove (player);
            bool sameTeam = true;
            int team = PlayerTeams[0];
            for (int i=0; i < PlayerTeams.Count; i++)
            {
                if (PlayerTeams[i] != team)
                {
                    sameTeam = false;
                }
            }
			if (sameTeam) {
				EndGame ();
			}
		}
	}

	IEnumerator SpawnPlayerAfter(IDictionary<string,string> playerKeys, Vector3 SpawnPosition, int playerI, string name)
  {
		Instantiate (PhaseInEffect, UtilityScript.transformToCartesian (SpawnPosition), Quaternion.identity);
		yield return new WaitForSeconds (PhaseInDelay);
		SoundManager.PlaySpawnClip ();
		GameObject localPlayer = MonoBehaviour.Instantiate (localPlayerPrefabs[(playerI % (localPlayerPrefabs.Length))], SpawnPosition, new Quaternion ()) as GameObject;
		LivingPlayers.Add (localPlayer);
        PlayerTeams.Add(playerI);
		//SpriteRenderer PlayerSR = localPlayer.GetComponentInChildren<SpriteRenderer> ();
		//Debug.Log ("GETTING COLOR AT INDEX " + playerI);
		//PlayerSR.color = playerColors[(playerI % (playerColors.Count))];
		LocalPlayerController LCP = localPlayer.GetComponent<LocalPlayerController> ();
		LCP.setKeys (playerKeys);
		LCP.PlayerName = name;

    cameraLoc.updatePlayers = true;

	  if(usePowerUps)
  		  PUS.spawnPowerups = true;
  }

	IEnumerator MultiSpawnPlayerAfter(Vector3 SpawnPosition, int playerI, LocalPlayerSender LPS)
	{
		Instantiate (PhaseInEffect, UtilityScript.transformToCartesian(SpawnPosition), Quaternion.identity);
		yield return new WaitForSeconds (PhaseInDelay);
		SoundManager.PlaySpawnClip ();
		GameObject localPlayer = MonoBehaviour.Instantiate (localPlayerPrefabs[(playerI % (localPlayerPrefabs.Length))], SpawnPosition, new Quaternion ()) as GameObject;
		LivingPlayers.Add (localPlayer);
		//SpriteRenderer PlayerSR = localPlayer.GetComponentInChildren<SpriteRenderer> ();
		//Debug.Log ("GETTING COLOR AT INDEX " + playerI);
		//PlayerSR.color = playerColors[(playerI % (playerColors.Count))];
		LocalPlayerController LCP = localPlayer.GetComponent<LocalPlayerController> ();
		LCP.PlayerName = playerNames [playerI % playerNames.Length];
		LPS.myPlayerOnTheServer = LCP;
		cameraLoc.updatePlayers = true;
		if(usePowerUps)
			PUS.spawnPowerups = true;
	}

	public void FinalDestruction(float delayStep){
		// disable power up spawner
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

	void EndGame(){
		gameEnded = true;
		FinalDestruction (7.5f);
		if (LivingPlayers.Count > 0) {
			LocalPlayerController LPC = LivingPlayers [0].GetComponent<LocalPlayerController> ();
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
			
			GameObject playerMesh = Instantiate (LPC.mesh, LPC.mesh.transform.position, LPC.mesh.transform.rotation) as GameObject;
			Destroy (LPC.gameObject);
			WCC.FinishGame (playerMesh, PName);
		}
	}
	public void ClickServerStartGame(){
		SpawnRings (RingSizes);
		MultiSpawnPlayers ();
		SoundManager.PlayBigBoomClip ();
	}
}
