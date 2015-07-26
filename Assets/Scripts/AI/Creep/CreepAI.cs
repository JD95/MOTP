using UnityEngine;
using System.Collections.Generic;

public abstract class CreepAI : MonoBehaviour
{
	protected List<AI_Objective> secondary_Objectives;
	protected Stack<AI_Objective> active_Objectives;

	protected AI_Objective main_Objective;

	// This is where the creep's secondary objectives are added
	protected abstract void fillSecondaryObjectives();
	

	// Creates and disables an objective for the current game object
	protected T createObjective<T>() where T : AI_Objective
	{
		var objective = gameObject.AddComponent<T>();
		objective.GetComponent<T>().enabled = false;
		objective.init();

		return objective;
	}

	protected void runObjectives()
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
	protected void pushObjective(AI_Objective objective)
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
	protected void popObjective()
	{
		// Turn off previous objective
		active_Objectives.Peek().turnOff();
		
		// Pop the main objective back into secondary objectives
		secondary_Objectives.Add(active_Objectives.Pop());
		
		// Activate previous objective
		active_Objectives.Peek().turnOn();
	}

}