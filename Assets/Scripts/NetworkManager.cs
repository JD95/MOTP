using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : Photon.MonoBehaviour {

    GameManager gameManager;

    public const string ROOM_NAME = "main";
    public static int playerCount = 0;   

    private bool connected = false;

	public int playerID;

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
        //PhotonNetwork.JoinRoom("main", true);
        PhotonNetwork.JoinOrCreateRoom("main", new RoomOptions(), new TypedLobby());
	}

	void OnJoinedRoom ()
	{
		Debug.Log("Player Connected");
        playerID = ++playerCount;
        Debug.Log("Their ID is = " + playerID.ToString());
	}
	
}
