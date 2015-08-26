using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.MonoBehaviour
{
    public const int WAVE_INTERVAL = 30;
    public string currentHero;
	
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

        // Connect to server

	}
	
	public void InitGame() {
		paused = false;
		init = true;
		SpawnPlayer ();
	}
	
	
	public void SpawnPlayer ()
	{
		GameObject mySpawn = bluespawn[UnityEngine.Random.Range(0,bluespawn.Length)];
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
        // Display Menu

		spawnWaves();
	}

	private void spawnWaves()
	{

		if (init) {
            if (time >= WAVE_INTERVAL)
            {
                spawnCreepWave("Creep_TeamA", 3, "Creep(ranged)_TeamA", 1, bluespawn);
                spawnCreepWave("Creep_TeamB", 3, "Creep(ranged)_TeamB", 1, redspawn);
				
				time = 0;
			}
			time += Time.deltaTime;
		}
	}

    delegate void spawner(string type);
    private void spawnCreepWave(string meleeCreep, int meleeAmount, string rangedCreep, int rangedAmount, GameObject[] spawnPoints)
    {
        int waveCount =  meleeAmount + rangedAmount;

        for (int i = 0; i < waveCount; i++)
        {
            string   spawnName  = spawnPoints[i % spawnPoints.Length].name;
            Waypoint spawnLoc   = GameObject.Find(spawnName).GetComponent<Waypoint>();
            spawner  spawn      = (x) => { SpawnCreep(x, spawnLoc); };

            if (i <= meleeAmount) spawn(meleeCreep); else spawn(rangedCreep);
        }
    }

	private void SpawnCreep(String prefab, Waypoint spawn) {
		PhotonNetwork.Instantiate (prefab, 
		                           spawn.transform.position, 
		                           spawn.transform.rotation, 0)
			         .GetComponent<Character>().charID = charNumber++;
	}
	
}
