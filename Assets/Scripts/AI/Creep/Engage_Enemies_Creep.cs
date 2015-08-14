using UnityEngine;
using System.Collections;

public class Engage_Enemies_Creep : Engage_Enemies {

	protected override void handle_OutofRange()
	{
		//Debug.Log ("I am creep and I am moving towards " + combatData.target.location.name);
		// Persue target

		if(combatData.target.transform != null){
			nav.moveTo(combatData.target.transform.position, combatData.attackRange());
		}
	}
}
