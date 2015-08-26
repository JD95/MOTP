using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : Photon.MonoBehaviour {

    public const string ROOM_NAME = "main";
    private bool connected = false;

	public int connectionPort = 25001;
	string connectionIP = "127.0.0.1";
	string jogadores = "2";
	public GUISkin skin;

	GameManager gameManager;
	
	bool inGame = false;
	private int playersConnected = 0;
	private PhotonPlayer[] networkPlayers;
	public static int playerCount = 0;

	private int playerID;
	
	public delegate bool gameJoin();

	// Use this for initialization
	void Start () {

		PhotonNetwork.ConnectUsingSettings("v1.0");

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnJoinedLobby()
	{
        connected = true;
	}

	void OnJoinedRoom ()
	{
		Debug.Log("Player Connected");

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
		
		GUI.Label (new Rect (0, 80, 40, 30), "IP: ");
		connectionIP = GUI.TextField (new Rect (50, 80, 100, 30), connectionIP);

		if (connected && GUI.Button (new Rect (160, 80, 240, 30), "Join Game")) {
            PhotonNetwork.JoinRoom(ROOM_NAME,true);
			inGame = true;
		}
		
		GUI.EndGroup();
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
