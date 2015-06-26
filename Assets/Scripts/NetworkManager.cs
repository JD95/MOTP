using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : Photon.MonoBehaviour {


	public int connectionPort = 25001;
	string connectionIP = "127.0.0.1";
	string jogadores = "2";
	public GUISkin skin;

	GameManager gameManager;
	
	Boolean inGame;
	private int playersConnected = 0;
	private PhotonPlayer[] networkPlayers;
	public static int playerCount = 0;

	private int playerID;
	
	public delegate bool gameJoin();

	// Use this for initialization
	void Start () {

		PhotonNetwork.ConnectUsingSettings("0.1");
		inGame = false;

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {
		// Displays connection status on screen
		//GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby()
	{
		//PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");

		// Creates opens the server for us to play on
		PhotonNetwork.CreateRoom(null);
	}

//	void OnJoinedRoom ()
//	{
//		networkPlayers = new PhotonPlayer[players];
//		networkPlayers[++playersConnected] = PhotonNetwork.player;
//		if (playersConnected >= players) {
//			gameManager.InitGame();
//		}
//	}

	void OnJoinedRoom ()
	{
		Debug.Log("Player Connected");

//		networkPlayers[playersConnected++] = player;
//		if (playersConnected >= playerCount) {
//			gameManager.InitGame(networkPlayers, playerCount);
//		}

		playerID = playerCount++;
		gameManager.InitGame();
	}
	
	void OnGUI ()
	{
		GUI.skin = skin;
		if (inGame == false) displayGameMenu(); 
		else displayDisconnectButton(); 
	}

	void displayGameMenu()
	{
		GUI.BeginGroup (new Rect (Screen.width/2 - 200, Screen.height/2 - 100, 400, 200));

		GUI.Label (new Rect (0, 0, 400, 30), "Status: Disconnected");

		GUI.Label (new Rect (0, 40, 100, 30), "Players: ");

		jogadores = GUI.TextField (new Rect (100, 40, 50, 30), jogadores);
		
		if (GUI.Button (new Rect (160, 40, 240, 30), "Create Server")) {
			playerCount = Convert.ToInt32 (jogadores);

			createNewGame(); // Does same thing as join game for now
		}
		
		GUI.Label (new Rect (0, 80, 40, 30), "IP: ");
		connectionIP = GUI.TextField (new Rect (50, 80, 100, 30), connectionIP);

		if (GUI.Button (new Rect (160, 80, 240, 30), "Join Game")) {
			PhotonNetwork.JoinRandomRoom();
			inGame = true;
		}
		
		GUI.EndGroup();
	}

	void createNewGame()
	{
		//PhotonNetwork.CreateRoom(null);
		PhotonNetwork.JoinRandomRoom();
		//PhotonNetwork.InstantiateSceneObject("GameManager",Vector3.zero,Quaternion.identity,0,null);
		inGame = true;
	}

	void displayDisconnectButton()
	{
		GUI.Label (new Rect (10, Screen.height - 40, 300, 30), "Status: Connected as Client");
		if (GUI.Button (new Rect (310, Screen.height - 40, 150, 30), "Disconnect")) {
			PhotonNetwork.Disconnect ();
			
			// Restart the game
			Application.LoadLevel ("Prototype Moba Map");
		}
	}
	
}
