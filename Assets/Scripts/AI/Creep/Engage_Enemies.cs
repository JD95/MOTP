using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Engage_Enemies : AI_Objective {

	private Combat combatData;
	private Character character;

	/* 	Keeps track of all enemies within fight range
	 * 	While enemies are within range, the character
	 * 	will persue the target for some range before 
	 * 	giving up
	 * 
	 * */
	List<Target> inRangeEnemies;

	public override void init()
	{
		inRangeEnemies = new List<Target>();

		combatData = gameObject.GetComponent<Combat>();
		character = gameObject.GetComponent<Character>();
	}


	void OnTriggerEnter (Collider _other) {

		//Debug.Log ("Object hit my radius!");

		Target other = _other.GetComponentInParent<Combat>().self;

		if (other.Equals(gameObject.transform))
			return;

		if (isEnemy(other) && !inList(other)) {

			//Debug.Log ("Adding " + other.location.name);

			if (noCurrentTarget()){
				//Debug.Log ("New target Selected!");
				combatData.target = other; // Make new target
			}

			inRangeEnemies.Add(other);
		}
	}

	bool isEnemy(Target other)
	{
		//Debug.Log ("Tag 1: " + other.tag + " Tag2: " + transform.tag);
		return other.location.tag != gameObject.transform.tag;
	}

	bool noCurrentTarget()
	{
		return combatData.target == null;
	}

	bool inList(Target other)
	{
		return inRangeEnemies.Contains(other);
	}

	bool enemiesInRange()
	{
		return inRangeEnemies.FindAll(x => x.selectable && !x.dead).Count() != 0;
	}

	void OnTriggerExit (Collider _other) {

		Target other = _other.GetComponentInParent<Combat>().self;

		// If the target goes out of range change target
		if (!other.dead && isEnemy (other) && inList(other))
		{
			// Deselect that enemy
			if (other != combatData.target)
			{
				combatData.target = null;
			}

			// Remove from in rage enemies
			inRangeEnemies.Remove(other);
		}

	}

	// Tells the AI to begin this objective
	public override bool begin()
	{
		return enemiesInRange();
	}

	public override void progress()
	{
		//Debug.Log("AI progressing");

		if (combatData.target != null && !combatData.targetWithin_AttackRange())
		{
			//Debug.Log ("Moving towards target!");
			// Persue target
			character.moveTo(combatData.target.location);
			
		}else if(combatData.target == null || combatData.target.dead){
				
			//Debug.Log("My target is dead!");
			inRangeEnemies.Remove(combatData.target);
			combatData.target = inRangeEnemies.Find(x => x.selectable && !x.dead);

		}else if(!combatData.target.selectable){

			// Finds the first target that is both selectable and not dead
			combatData.target = inRangeEnemies.Find(x => x.selectable && !x.dead);


		}else{
			//Debug.Log ("Auto attack go!");
			combatData.autoAttack();
		}
	}

	// Tells the AI that the objective is complete
	public override bool end()
	{
		// If there are no enemies in range then nothing to attack
		return !enemiesInRange();
	}


}
