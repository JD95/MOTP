using UnityEngine;
using System.Collections;

public abstract class AI_Objective : MonoBehaviour{

	public bool objectiveActive = false;

	/*
	 * The equivalent of a Start() function, it is
	 * needed because Unity will not call start given
	 * that the objectives are created dynamically
	 */
	public abstract void init();

	/*
	 * This function will return true if all conditions
	 * are met for the objective to begin. For example,
	 * an engage enemies objective would begin when there
	 * are enemies near
	 */
	public abstract bool begin();

	/*
	 * Throughout the course of achieving the objective
	 * the NPC will carry out these steps. They could include
	 * adjusting the destination, changing target, casting an ability, etc.
	 */
	public abstract void progress();

	/*
	 * Returns true if the conditions are met for the objective
	 * to end. The engage enemies objective would end if there are
	 * no longer any enemies within range or something to that effect.
	 */
	public abstract bool end();

	public bool isActive()
	{
		return objectiveActive;
	}

	public void turnOff()
	{
		objectiveActive = false;
	}
	
	public void turnOn()
	{
		objectiveActive = true;
	}

}