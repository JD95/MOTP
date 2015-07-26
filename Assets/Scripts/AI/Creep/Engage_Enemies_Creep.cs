using UnityEngine;
using System.Collections;

public class Engage_Enemies_Creep : Engage_Enemies {

	protected override void handle_OutofRange()
	{
		//Debug.Log ("Moving towards target!");
		// Persue target
		character.moveTo(combatData.target.location);
	}
}
