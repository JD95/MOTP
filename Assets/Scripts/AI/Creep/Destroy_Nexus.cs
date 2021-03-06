﻿using UnityEngine;
using System.Collections;

public class Destroy_Nexus : AI_Objective {

	Combat combatData;
	Transform nexus;
	NavMeshAgent movement;

	public override void init()
	{
		combatData = gameObject.GetComponent<Combat>();
		movement = gameObject.GetComponent<NavMeshAgent>();
		
		nexus = GameObject.Find(targetNexus()).GetComponent<Transform>();
	}

	public override bool begin()
	{
		// This is a default behaviour, don't really need to define this
		return true;
	}

	private string targetNexus()
	{
		if(gameObject.CompareTag("teamA"))
		{
			return "NexusB";
		}
		else{
			return "NexusA";
		}
	}

	public override void progress()
	{
		if(!movement.destination.AlmostEquals(nexus.position, 1.0F))
		{
			//Debug.Log("Target is now set to nexus");
			movement.SetDestination(nexus.position);
			movement.Resume();
		}

		// When in range of Nexus auto attack it
	}

	public override bool end()
	{
		// Nexus is destroyed
		return false;
	}


}
