using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Linq;

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
	static int[] teamScores;

	float PhaseInDelay = float.MaxValue;

  public GameObject PhaseInEffect;
  //public GameObject localPlayerPrefab;
  public GameObject[] localPlayerPrefabs;
  private int[] prefabInd;
  public WinnerCanvasController WCC;
  public lobbyScript lobby;

	public Text[] playerTexts;

	public int[] teams;

	//MULTIPLAYER
	public bool multiplayerMode = false;
	public List<LocalPlayerSender> LocalPlayerSenders = new List<LocalPlayerSender>();


	// powerups
	PowerUpSpawner PUS;
	public bool usePowerUps;
  EnvironmentManager EM;
  public bool useEnvironmentEffects;
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

        Debug.Log ("loading Map " + lobby.GetSelectedMap ());
		SpawnRings (lobby.GetSelectedMap());
		SpawnPlayers (NumPlayers);
		SelectPowerUpSpawner (lobby.GetSelectedMap ());
    SelectEnvironmentManager (lobby.GetSelectedMap());
    SoundManager.play_big_boom ();

    // powerups
  }

	void SelectPowerUpSpawner(string arenaname){
		ArenaData ad = ArenaDataLoader.arenas [arenaname];
		if (ad.powerupSpawner == null)
			PUS = new PowerUpSpawner ();
		else
			PUS = new PowerUpSpawner (this, ad.powerupSpawner.powerUps, ad.powerupSpawner.spawnDistances, ad.powerupSpawner.minSpawnDuration, ad.powerupSpawner.maxSpawnDuration, ad.powerupSpawner.maxNumberOfPowerups);
		//PUS = new PowerUpSpawner(ad.
	}

  void SelectEnvironmentManager(string arenaname)
  {
    ArenaData ad = ArenaDataLoader.arenas[arenaname];
    if (ad.environmentEffectsData == null)
      EM = new EnvironmentManager();
    else
      EM = new EnvironmentManager(this, ad.environmentEffectsData.environmentEffects, ad.environmentEffectsData.maxSpawnDuration, ad.environmentEffectsData.maxSpawnDuration, ad.environmentEffectsData.minNoOfEffects, ad.environmentEffectsData.maxNoOfEffects);
  }

  void Awake ()
	{
		GMInstance = this;
	}

	void Start ()
	{
		playerNames = new string[4];
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
		teams = lobby.getTeams ();
		lobby.hideLobby();

		teamScores = new int[lobby.getTeams().Length];
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
			playerTexts [i].text = "";
		}

		int NumberOfPlayers = NumPlayers;

		for (int i = 0; i < NumberOfPlayers; i++) {
			playerTexts [teams [i]].text += playerNames [i] + ", ";
			playerTexts [teams [i]].enabled = true;
		}
		for (int i = 0; i < NumberOfPlayers; i++) {
			if (playerTexts [i].text.EndsWith (", ")) {
				playerTexts [i].text = playerTexts [i].text.Substring (0, playerTexts [i].text.Length - 2) + ": 0";
			}
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
	}

	// Update is called once per frame

	void Update ()
	{
		if (toStart) {
			StartGame ();
			toStart = false;
		}
    if (!gameEnded)
    {
      PUS.Update();
      EM.Update();
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
		//SpawnPositions = CalculateSpawnPositions (NumberOfPlayers);
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
        p1keys.Add("movementKey", "P1MovementKeyboard");
        p1keys.Add("gravityChange", "P1Jump");
        p1keys.Add("shoot", "P1Shoot");

        IDictionary<string, string> p2keys = new Dictionary<string, string>();
        p2keys.Add("movement", "P2Movement");
        p2keys.Add("movementKey", "P2MovementKeyboard");
        p2keys.Add("gravityChange", "P2Jump");
        p2keys.Add("shoot", "P2Shoot");

        IDictionary<string, string> p3keys = new Dictionary<string, string>();
        p3keys.Add("movement", "P3Movement");
        p3keys.Add("movementKey", "P3MovementKeyboard");
        p3keys.Add("gravityChange", "P3Jump");
        p3keys.Add("shoot", "P3Shoot");

        IDictionary<string, string> p4keys = new Dictionary<string, string>();
        p4keys.Add("movement", "P4Movement");
        p4keys.Add("movementKey", "P4MovementKeyboard");
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
			StartCoroutine(SpawnPlayerAfter(keyCodes[i], ArenaDataLoader.arenas["basic"].getSpawnPosition(NumberOfPlayers, i), prefabInd[i] ,playerNames[i], teams[i]));
		}
	}

	void MultiSpawnPlayers ()
	{
		//SpawnPositions = CalculateSpawnPositions (LocalPlayerSenders.Count);
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
			LivingPlayers.Remove (player);
			Dictionary<int, int> pteams = new Dictionary<int, int> ();
			for (int i = 0; i < LivingPlayers.Count; i++) {
				LocalPlayerController LCP = LivingPlayers[i].GetComponent<LocalPlayerController> ();
				try{
					pteams.Add(LCP.team, LCP.team);
				}catch(Exception e){
				}
			}
			int[] teams = pteams.Values.ToArray ();
			for (int i = 0; i < teams.Length; i++) {
				teamScores [teams [i]]++;
			}
			for (int i = 0; i < teams.Length; i++) {
				Text t = playerTexts [teams[i]];
				t.text = t.text.Substring (0, t.text.LastIndexOf (": ") + 2) + teamScores [teams [i]]; 
			}

			if (teams.Length < 2) {
				gameEnded = true;
				while (LivingPlayers.Count > 1) {
					GameObject go = LivingPlayers [LivingPlayers.Count - 1];
					LivingPlayers.RemoveAt (LivingPlayers.Count - 1);	
					go.GetComponent<LocalPlayerController> ().DestroyObject ();
				}
				EndGame ();
			}
		}
	}


	IEnumerator SpawnPlayerAfter (IDictionary<string,string> playerKeys, Vector2 SpawnPos, int playerI, string name, int team)
	{
		Vector3 SpawnPosition = new Vector3 (SpawnPos.x, SpawnPos.y, 0);
		GameObject pie = Instantiate (PhaseInEffect, UtilityScript.transformToCartesian (SpawnPosition), Quaternion.identity) as GameObject;
		pie.transform.SetParent (GameManager.GMInstance.transform);
		yield return new WaitForSeconds (PhaseInDelay);
    SoundManager.play_spawn ();
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

		//playerTexts [team].text = LCP.PlayerName + ", ";
		//playerTexts [team].enabled = true;

		cameraLoc.updatePlayers = true;

		if (usePowerUps)
			PUS.spawnPowerups = true;

    if (useEnvironmentEffects)
      EM.enableEnvironmentEffects = true;
	}

	IEnumerator MultiSpawnPlayerAfter (Vector3 SpawnPosition, int playerI, LocalPlayerSender LPS)
	{
		GameObject pie = Instantiate (PhaseInEffect, UtilityScript.transformToCartesian (SpawnPosition), Quaternion.identity) as GameObject;
		pie.transform.SetParent (GameManager.GMInstance.root.transform);
		yield return new WaitForSeconds (PhaseInDelay);
    SoundManager.play_spawn ();
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

		//playerTexts [playerI].text = LCP.PlayerName + ": " + teamScores [playerI];
		//playerTexts [playerI].enabled = true;

		cameraLoc.updatePlayers = true;

		if (usePowerUps)
			PUS.spawnPowerups = true;

    if (useEnvironmentEffects)
      EM.enableEnvironmentEffects = true;
  }
	
	public void FinalDestruction (float delayStep)
	{
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
        int winningTeamInd = 0;
        int winnerScore = 0;

		for (int i=0; i < teamScores.Length; i++) {
			if(teamScores[i] >= winScore){
				scoreReached=true;
                
            }

            if (teamScores[i] > winnerScore)
            {
                winnerScore = teamScores[i];
                winningTeamInd = i;
            }
		}

		if (scoreReached) {

      //TODO: end round
      SoundManager.play_victory_song();
      string PName = "";
			if (LivingPlayers.Count == 1)
			{
                Text t = playerTexts[teams[winningTeamInd]];
                string sub = t.text.Substring(0, t.text.LastIndexOf(": "));
                PName = sub + " WINS!";
			}
			else
			{
                Text t = playerTexts[teams[winningTeamInd]];
                string sub = t.text.Substring(0, t.text.LastIndexOf(": "));
                PName = sub + " WIN!";
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
    SoundManager.play_big_boom ();
  }
}
