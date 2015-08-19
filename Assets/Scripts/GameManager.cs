using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.MonoBehaviour
{

    public string currentHero;

	// hero prefabs
	public Transform playerPrefabA;
	public Transform playerPrefabB;
	
	// creep prefabs
	public CreepAI creepPrefabA;
	public CreepAI creepPrefabB;
	
	// Hero Spawn Locations
	public GameObject[] redspawn;
	public GameObject[] bluespawn;

    public GameObject _playerStats;
    private StatsManager playerStats; 
	
	public static bool paused = true;
	
	public List<Hero> playerScripts = new List<Hero> ();
	private int charNumber = 0;
	
 	private bool init = false;

	public bool gameOver = false;

	void Start() {
		paused = true;
        playerStats = _playerStats.GetComponent<StatsManager>();
	}
	
	public void InitGame() {
		paused = false;
		//int count = 0;
		init = true;
		SpawnPlayer ();
	}
	
	
	public void SpawnPlayer ()
	{

		GameObject mySpawn = bluespawn[UnityEngine.Random.Range(0,redspawn.Length)];
        GameObject myPlayer = PhotonNetwork.Instantiate(currentHero, mySpawn.transform.position, mySpawn.transform.rotation, 0);
        myPlayer.name = "player";
		enablePlayer (myPlayer);
        playerStats.Init();

		GameObject.Find("Main Camera").GetComponent<CameraControl>().SetTarget(myPlayer.transform);
	}

	void enablePlayer(GameObject player)
	{
		// No one else can control our character except for us!
		player.GetComponent<Hero>().enabled = true;
        player.GetComponent<Abilities>().enabled = true;
		player.GetComponent<Character>().enabled = true;
		player.GetComponent<AudioSource>().enabled = true;
		player.GetComponent<CharacterController>().enabled = true;
		player.GetComponent<NetworkCharacter>().enabled = true;

	}

	private float time = 30;

	public void Update ()
	{
		spawnWaves();
	}

	private void spawnWaves()
	{
		if (init) {
			if (time >= 30) {
				//				// creep A
				SpawnCreep("Creep(ranged)_TeamA", GameObject.Find("blueSpawn1").GetComponent<Waypoint>(), 0);
				SpawnCreep("Creep_TeamA", GameObject.Find("blueSpawn2").GetComponent<Waypoint>(), 0);
				SpawnCreep("Creep_TeamA", GameObject.Find("blueSpawn3").GetComponent<Waypoint>(), 0);
				SpawnCreep("Creep_TeamA", GameObject.Find("blueSpawn4").GetComponent<Waypoint>(), 0);
				//				
				//				// creep B
				SpawnCreep("Creep(ranged)_TeamB", GameObject.Find("redSpawn1").GetComponent<Waypoint>(), 0);
				SpawnCreep("Creep_TeamB", GameObject.Find("redSpawn2").GetComponent<Waypoint>(), 0);
				SpawnCreep("Creep_TeamB", GameObject.Find("redSpawn3").GetComponent<Waypoint>(), 0);
				SpawnCreep("Creep_TeamB", GameObject.Find("redSpawn4").GetComponent<Waypoint>(), 0);
				
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
