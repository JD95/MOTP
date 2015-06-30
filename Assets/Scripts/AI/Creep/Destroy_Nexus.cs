using UnityEngine;
using System.Collections;

public class Destroy_Nexus : AI_Objective {

	public bool begin()
	{
		// This is a default behaviour, don't really need to define this
		return true;
	}

	public void progress()
	{
		// Move along waypoints towards nexus

		// When in range of Nexus auto attack it

	}

	public bool end()
	{
		// Nexus is destroyed
		return true;
	}
}
