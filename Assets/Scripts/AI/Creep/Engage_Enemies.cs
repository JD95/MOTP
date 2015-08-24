using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using TeamLogic = Utility.TeamLogic;

public enum AIState {Attacking, ChangingTarget, MovingIntoRange};

public abstract class Engage_Enemies : AI_Objective {

	protected Combat combatData;
	protected Navigation nav;

	public AIState state;
	public int flicker = 0;

	/* 	Keeps track of all enemies within fight range
	 * 	While enemies are within range, the character
	 * 	will persue the target for some range before 
	 * 	giving up
	 * 
	 * */
	protected List<GameObject> inRangeEnemies;
	public List<string> enemyNames;

	public override void init()
	{
		inRangeEnemies = new List<GameObject>();
		enemyNames = new List<string>();

		combatData = gameObject.GetComponent<Combat>();
		nav = gameObject.GetComponent<Navigation>();

	}
	
	protected void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == 8) return; // Non clickable object
		//Debug.Log(other.name + " is in my range!");

		Combat test;
		if (test = other.GetComponentInParent<Combat>())
		{
			//if (other.self.Equals(gameObject.transform)) return;

			if (TeamLogic.areEnemies(this.gameObject, other.gameObject) && !inList(other.gameObject)) {

				//Debug.Log ("Adding " + other.gameObject.name);

				if (noCurrentTarget()){
					//Debug.Log ("New target Selected!");
					combatData.target = other.gameObject; // Make new target
				}

				inRangeEnemies.Add(other.gameObject);
			}
		}
	}

	protected bool noCurrentTarget()
	{
		return combatData.target == null;
	}

	protected bool inList(GameObject other)
	{
		return inRangeEnemies.Contains(other);
	}

	protected bool enemiesInRange()
	{
		return inRangeEnemies.Any(x => x != null && x.transform != null && x.GetComponent<Combat>().selectable);
	}

	protected void OnTriggerExit (Collider other) {

		Combat test;

		if(test = other.gameObject.GetComponentInParent<Combat>())
		{

			// If the target goes out of range change target
			if (TeamLogic.areEnemies (this.gameObject, other.gameObject) && inList(other.gameObject))
			{
				// Deselect that enemy
				if (other.gameObject == combatData.target)
				{
					combatData.target = null;
				}

				// Remove from in rage enemies
				inRangeEnemies.Remove(other.gameObject);
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
		if(flicker == 0) {flicker = 1;}
		else {flicker = 0;}

		//Debug.Log("AI progressing");
        inRangeEnemies.RemoveAll(item => item == null); //|| item.transform == null || !item.GetComponent<Combat>().selectable);
		enemyNames = convertList();

		// Target alive but not in attack range
		if (combatData.target != null && combatData.target.transform != null && !combatData.targetWithin_AttackRange())
		{
			state = AIState.MovingIntoRange;

			//Creep is not in range
			if(nav != null)
			{nav.turnOff_withinRange();}
			handle_OutofRange();
		
		// Target cannot be attacked
		}else if(combatData.target == null || combatData.target.transform == null || !combatData.target.GetComponent<Combat>().selectable){
				
			//Debug.Log("Changing target!");
			state = AIState.ChangingTarget;

			inRangeEnemies.Remove(combatData.target);
			combatData.target = inRangeEnemies.Find(x => x.GetComponent<Combat>().selectable == true);

			//Debug.Log ("I am " + gameObject.name + ", and now targeting " + combatData.target.ToString());

		// Target cam be attacl amd is in range
		}else{
			state = AIState.Attacking;
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
			inRangeEnemies.Clear();
			//Debug.Log(gameObject.name + " has no more enemies!");
			return true;
		}else{
			return false;
		}
	}


}
