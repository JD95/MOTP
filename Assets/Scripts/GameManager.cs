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
		GameObject mySpawn = redspawn[UnityEngine.Random.Range(0,redspawn.Length)];
		GameObject myPlayer = PhotonNetwork.Instantiate("HeroPrefabA", mySpawn.transform.position, mySpawn.transform.rotation,0);

		// No one else can control our character except for us!
		myPlayer.GetComponent<Hero>().enabled = true;
		myPlayer.GetComponent<Character>().enabled = true;
		myPlayer.GetComponent<AudioSource>().enabled = true;
		myPlayer.GetComponent<CharacterController>().enabled = true;
		myPlayer.GetComponent<NetworkCharacter>().enabled = true;

		GameObject.Find("Main Camera").GetComponent<CameraControl>().SetTarget(transform);
	}
	
	private float time = 15;

	public void Update ()
	{
		if (init) {
			if (time >= 15) {
//				// creep A
//				SpawnCreep(creepPrefabA, spawnA, 0);
//				SpawnCreep(creepPrefabA, spawnA, -2);
//				SpawnCreep(creepPrefabA, spawnA, -4);
//				
//				// creep B
//				SpawnCreep(creepPrefabB, spawnB, 0);
//				SpawnCreep(creepPrefabB, spawnB, 2);
//				SpawnCreep(creepPrefabB, spawnB, 4);
//				time = 0;
			}
			time += Time.deltaTime;
		}
	}
	
	private void SpawnCreep(CreepAI prefab, Waypoint spawn, int offset) {
		var creep = PhotonNetwork.Instantiate ("CreepPrefabB", spawn.transform.position + new Vector3(offset,0,offset), spawn.transform.rotation, 0).GetComponent<CreepAI>();
		creep.nextWaypoint = spawn;
		creep.GetComponent<Character>().charID = charNumber++;
	}
	
}
