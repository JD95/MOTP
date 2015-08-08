using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Engage_Enemies : AI_Objective {

	protected Combat combatData;
	protected Navigation nav;

	/* 	Keeps track of all enemies within fight range
	 * 	While enemies are within range, the character
	 * 	will persue the target for some range before 
	 * 	giving up
	 * 
	 * */
	protected List<Target> inRangeEnemies;
	public List<string> enemyNames;

	public override void init()
	{
		inRangeEnemies = new List<Target>();
		enemyNames = new List<string>();

		combatData = gameObject.GetComponent<Combat>();
		nav = gameObject.GetComponent<Navigation>();

	}
	
	protected void OnTriggerEnter(Collider _other) {

		if(_other.name == "AI_Collider") return;
		//Debug.Log(_other.name + " is in my range!");

		Combat other;
		if (other = _other.GetComponent<Combat>())
		{
			//if (other.self.Equals(gameObject.transform)) return;

			if (isEnemy(other.self) && !inList(other.self)) {

				//Debug.Log ("Adding " + other.gameObject.name);

				if (noCurrentTarget()){
					//Debug.Log ("New target Selected!");
					combatData.target = other.self; // Make new target
				}

				inRangeEnemies.Add(other.self);
			}
		}
	}

	protected bool isEnemy(Target other)
	{
		//Debug.Log ("Tag 1: " + other.tag + " Tag2: " + transform.tag);
		return other.location.tag != gameObject.tag;
	}

	protected bool noCurrentTarget()
	{
		return combatData.target == null;
	}

	protected bool inList(Target other)
	{
		return inRangeEnemies.Contains(other);
	}

	protected bool enemiesInRange()
	{
		return inRangeEnemies.Any(x => x.selectable && !x.dead);
	}

	protected void OnTriggerExit (Collider _other) {

		Combat other;

		if(other = _other.GetComponentInParent<Combat>())
		{

			// If the target goes out of range change target
			if (!other.self.dead && isEnemy (other.self) && inList(other.self))
			{
				// Deselect that enemy
				if (other.self != combatData.target)
				{
					combatData.target = null;
				}

				// Remove from in rage enemies
				inRangeEnemies.Remove(other.self);
			}
		}

	}

	// Tells the AI to begin this objective
	public override bool begin()
	{
		if(nav != null && enemiesInRange())
		{
			// Creep is in combat
			nav.turnOn_inCombat();
		}

		return enemiesInRange();
	}

	private List<string> convertList()
	{	
		List<string> newList = new List<string>();
		inRangeEnemies.ForEach(x => newList.Add(x.ToString()));

		return newList;
	}

	public override void progress()
	{
		//Debug.Log("AI progressing");
		inRangeEnemies.RemoveAll(item => item == null || item.location == null);
		enemyNames = convertList();

		// Target alive but not in attack range
		if (combatData.target != null && !combatData.targetWithin_AttackRange())
		{
			//Creep is not in range
			if(nav != null)
			{nav.turnOff_withinRange();}
			handle_OutofRange();
		
		// Target cannot be attacked
		}else if(combatData.target == null || combatData.target.location == null || combatData.target.dead || !combatData.target.selectable){
				
			Debug.Log("Changing target!");
			inRangeEnemies.Remove(combatData.target);
			combatData.target = inRangeEnemies.Find(x => x.selectable && !x.dead);
		
		// Target cam be attacl amd is in range
		}else{

			//Creep is within range
			if(nav != null)
			{nav.turnOn_withinRange();}

			//Debug.Log ("Auto attack go!");
			combatData.autoAttack();
		}
	}

	protected abstract void handle_OutofRange();

	// Tells the AI that the objective is complete
	public override bool end()
	{
		// If there are no enemies in range then nothing to attack
		if (nav != null && !enemiesInRange ())
		{
			nav.turnOff_inCombat();
		}

		return !enemiesInRange();
	}


}
