using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Engage_Enemies : Photon.MonoBehaviour, AI_Objective {

	private Combat combatData;
	private Character character;

	/* 	Keeps track of all enemies within fight range
	 * 	While enemies are within range, the character
	 * 	will persue the target for some range before 
	 * 	giving up
	 * 
	 * */
	List<Transform> inRangeEnemies;

	public void Start()
	{
		inRangeEnemies = new List<Transform>();
		combatData = GetComponent<Combat>();
		character = GetComponent<Character>();
	}

	void OnTriggerStay (Collider _other) {
		var other = _other.GetComponent<Transform>();

		if (other == transform)
			return;

		if (isEnemy(other) && !inList(other)) {

			Debug.Log ("Adding " + other.name);

			if (noCurrentTarget()){
				Debug.Log ("New target Selected!");
				combatData.target = other; // Make new target
			}

			inRangeEnemies.Add(other);
		}
	}

	bool isEnemy(Transform other)
	{
		//Debug.Log ("Tag 1: " + other.tag + " Tag2: " + transform.tag);
		return other.tag != transform.tag;
	}

	bool noCurrentTarget()
	{
		return combatData.target == null;
	}

	bool inList(Transform other)
	{
		return inRangeEnemies.Contains(other);
	}

	bool enemiesInRange()
	{
		return inRangeEnemies.Count() == 0;
	}

	void OnTriggerExit (Collider _other) {

		var other = _other.GetComponent<Transform>();

		// If the target goes out of range change target
		if (isEnemy (other) && inList(other))
		{
			// Deselect that enemy
			if (other.transform != combatData.target)
				combatData.target = null;

			// Remove from in rage enemies
			inRangeEnemies.Remove(other);
		}

	}

	// Tells the AI to begin this objective
	public bool begin()
	{
		return enemiesInRange();
	}

	// Take this action while competing the objective
	public void Update () // Just for testing
	{
		if (!combatData.targetWithin_AttackRange())
		{
			// Persue target
			if(combatData.target != null)
				character.moveTo(combatData.target);

		}else{
			//if(combatData.target != null) // Just for testing
				combatData.autoAttack();
		}
	}

	public void progress()
	{

	}

	// Tells the AI that the objective is complete
	public bool end()
	{
		// If there are no enemies in range then nothing to attack
		return !enemiesInRange();
	}
}
