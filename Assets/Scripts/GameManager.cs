using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.MonoBehaviour
{
	
	// hero prefabs
	public Transform playerPrefabA;
	public Transform playerPrefabB;
	
	// creep prefabs
	public CreepAI creepPrefabA;
	public CreepAI creepPrefabB;
	
	// Hero Spawn Locations
	public GameObject[] redspawn;
	public GameObject[] bluespawn;
	
	public static bool paused = true;
	
	public List<Hero> playerScripts = new List<Hero> ();
	private int charNumber = 0;
	
 	private bool init = false;
	
	void Start() {
		paused = true;
	}
	
	public void InitGame(/*PhotonPlayer[] networkPlayers, int players */) {
		paused = false;
		int count = 0;
		init = true;
		SpawnPlayer (/*playerPrefabA, player, spawnA.transform.position + new Vector3(-2-2*count,0.5f,0)*/);
	}
	
	
	public void SpawnPlayer (/*Transform prefab, PhotonPlayer player, Vector3 position */)
	{
//		string tempPlayerString = player.ToString ();
//		int playerNumber = Convert.ToInt32 (tempPlayerString);
//
//		var newPlayer = PhotonNetwork.Instantiate ("HeroPrefabA", position, transform.rotation, playerNumber);
//		newPlayer.transform.GetComponent<Character>().charID = charNumber++;
//
//
//		GameObject.Find("Main Camera").GetComponent<CameraControl>().SetTarget(newPlayer.transform);
		GameObject mySpawn = bluespawn[UnityEngine.Random.Range(0,redspawn.Length)];
		GameObject myPlayer = PhotonNetwork.Instantiate("HeroPrefabA", mySpawn.transform.position, mySpawn.transform.rotation,0);

		enablePlayer (myPlayer);

		GameObject.Find("Main Camera").GetComponent<CameraControl>().SetTarget(myPlayer.transform);
	}

	void enablePlayer(GameObject player)
	{
		// No one else can control our character except for us!
		player.GetComponent<Hero>().enabled = true;
		player.GetComponent<Character>().enabled = true;
		player.GetComponent<AudioSource>().enabled = true;
		player.GetComponent<CharacterController>().enabled = true;
		player.GetComponent<NetworkCharacter>().enabled = true;

	}

	private float time = 30;

	public void Update ()
	{
		if (init) {
			if (time >= 30) {
//				// creep A
				SpawnCreep("Creep(ranged)_TeamA", GameObject.Find("blueSpawn1").GetComponent<Waypoint>(), 0);
//				
//				// creep B
				SpawnCreep("Creep(ranged)_TeamB", GameObject.Find("redSpawn1").GetComponent<Waypoint>(), 0);

				time = 0;
			}
			time += Time.deltaTime;
		}
	}
	
	private void SpawnCreep(String prefab, Waypoint spawn, int offset) {
		PhotonNetwork.Instantiate (prefab, 
		                           spawn.transform.position + new Vector3(offset,0,offset), 
		                           spawn.transform.rotation, 0)
			.GetComponent<Character>().charID = charNumber++;
	}
	
}
