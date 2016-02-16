using UnityEngine;
//using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	private static Main instance;

	public static Main getInstance(){
		return instance;
	}

	public GameObject gamePrefab;

	int numberOfPlayers = 2;
	bool gameRunning = false;
	GameObject game = null;
	GameManager gameManager = null;
	
	public GameObject lobby;
	void Start () {
		instance = this;
	}

	public void startGame(){
		lobby.GetComponent<lobbyScript> ().hideLobby ();
		game = MonoBehaviour.Instantiate (gamePrefab) as GameObject;
		gameManager = game.GetComponentInChildren<GameManager> ();
		gameManager.NumPlayers = lobby.GetComponent<lobbyScript> ().getNumOfPlayers ();
		gameManager.lobby = lobby.GetComponent<lobbyScript>();
		gameRunning = true;
	}

	void Update(){/*
		if (!gameRunning) {
			if (game != null) {
				MonoBehaviour.Destroy (game);
				gameManager = null;
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				game = MonoBehaviour.Instantiate (gamePrefab) as GameObject;
				gameManager = game.GetComponentInChildren<GameManager> ();
				gameRunning = true;
				gameManager.NumPlayers = numberOfPlayers;
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				numberOfPlayers = 2;
			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				numberOfPlayers = 3;
			}
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				numberOfPlayers = 4;
			}
			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				numberOfPlayers = 5;
			}
		}
		if (game == null) {
			gameManager = null;
			gameRunning = false;
		}*/
	}

	public void nextRound(){
		gameManager.StartGame ();
	}

	public void backToLobby(){
		MonoBehaviour.Destroy (game);
		game = null;
		gameManager = null;
		gameRunning = false;
		lobby.GetComponent<lobbyScript> ().showLobby ();
	}
}
