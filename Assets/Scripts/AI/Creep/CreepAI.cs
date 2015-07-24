using UnityEngine;
using System.Collections.Generic;

public class CreepAI : MonoBehaviour
{
	List<AI_Objective> secondary_Objectives;
	Stack<AI_Objective> active_Objectives;

	AI_Objective main_Objective;
	
	void Start ()
	{
		active_Objectives = new Stack<AI_Objective>();
		secondary_Objectives = new List<AI_Objective>();

		// The main objective for creeps
		//main_Objective = createObjective<Destroy_Nexus>();
		main_Objective = createObjective<Destroy_Nexus>();
		active_Objectives.Push(main_Objective);

		// All other objectives
		fillSecondaryObjectives();

	}

	// This is where the creep's secondary objectives are added
	void fillSecondaryObjectives()
	{
		secondary_Objectives.Add(createObjective<Engage_Enemies>());
	}

	// Creates and disables an objective for the current game object
	T createObjective<T>() where T : AI_Objective
	{
		var objective = gameObject.AddComponent<T>();
		objective.GetComponent<T>().enabled = false;
		objective.init();

		return objective;
	}

	void Update ()
	{
		// Debug.Log(active_Objectives.Count.ToString());
		active_Objectives.Peek().turnOn();

		if(active_Objectives.Peek().end())
		{
			popObjective();
			//Debug.Log(gameObject.name + ": Objective Complete");
		}

		foreach(var objective in secondary_Objectives)
		{
			if(!objective.enabled && objective.begin())
			{
				pushObjective(objective);
				//Debug.Log(gameObject.name + ": Adding New Objective");
			}
		}

		active_Objectives.Peek().progress();

	}

	// When adding a new objective to the stack
	void pushObjective(AI_Objective objective)
	{
		// Turn off previous objective
		if(active_Objectives.Count != 0)
		active_Objectives.Peek().turnOff();

		// Add new objective
		active_Objectives.Push(objective);

		// Activate it
		active_Objectives.Peek().turnOn();
	}

	// When adding a new objective to the stack
	void popObjective()
	{
		// Turn off previous objective
		active_Objectives.Peek().turnOff();
		
		// Pop the main objective back into secondary objectives
		secondary_Objectives.Add(active_Objectives.Pop());
		
		// Activate previous objective
		active_Objectives.Peek().turnOn();
	}

}