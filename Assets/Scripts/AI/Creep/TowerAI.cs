using UnityEngine;
using System.Collections.Generic;

public class TowerAI : CreepAI {

	void Start ()
	{
		active_Objectives = new Stack<AI_Objective>();
		secondary_Objectives = new List<AI_Objective>();
		
		// The main objective for creeps
		//main_Objective = createObjective<Destroy_Nexus>();
		main_Objective = createObjective<Idle>();
		active_Objectives.Push(main_Objective);
		
		// All other objectives
		fillSecondaryObjectives();
		
	}

	void Update()
	{
		runObjectives();
	}

	// This is where the creep's secondary objectives are added
	protected override void  fillSecondaryObjectives()
	{
		secondary_Objectives.Add(createObjective<Engage_Enemies_Tower>());
	}
}
