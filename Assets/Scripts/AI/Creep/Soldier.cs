using UnityEngine;
using System.Collections.Generic;

public class Soldier : CreepAI {

	void Start ()
	{
		active_Objectives = new Stack<AI_Objective>();
		secondary_Objectives = new List<AI_Objective>();
		
		// The main objective for creeps
		//main_Objective = createObjective<Destroy_Nexus>();
		main_Objective = createObjective<Destroy_Nexus>();
		main_Objective.turnOn();
		active_Objectives.Push(main_Objective);
		
		// All other objectives
		fillSecondaryObjectives();
		
	}

	void Update()
	{
		runObjectives();
	}

	void OnDestroy()
	{
		var objectives = gameObject.GetComponents<AI_Objective>();

		foreach(var objective in objectives)
		{
			GameObject.Destroy(objective);
		}
	}

	// This is where the creep's secondary objectives are added
	protected override void  fillSecondaryObjectives()
	{
		//secondary_Objectives.Add(createObjective<Destroy_Nexus>());
		secondary_Objectives.Add(createObjective<Engage_Enemies_Creep>());
	}
}
