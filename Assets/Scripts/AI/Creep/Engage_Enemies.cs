using UnityEngine;
using System.Collections;

public class Engage_Enemies : AI_Objective {

	public bool begin()
	{
		// Check if enemy units are nearby
		return true;
	}

	public void progress ()
	{
		// Target nearby enemy
		// Auto attack them until they die
	}

	public bool end()
	{
		// No more enemies nearby
		return true;
	}
}
