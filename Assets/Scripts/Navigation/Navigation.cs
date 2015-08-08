using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	private float speed;
	private NavMeshAgent navAgent;

	public bool inCombat;

	public void turnOn_inCombat()
	{
		inCombat = true;

		updateMoving();
	}

	public void turnOff_inCombat()
	{
		inCombat = false;

		updateMoving();
	}

	public bool withinRange;

	public void turnOn_withinRange()
	{
		withinRange = true;

		updateMoving();
	}

	public void turnOff_withinRange()
	{
		withinRange = false;

		updateMoving();
	}

	bool hasObjectiveDestination;
	Vector3 objectiveDestination;

	public void turnOn_ObjectiveDestination(Vector3 destination)
	{
		hasObjectiveDestination = true;
		objectiveDestination = destination;

		updateMoving();
	}

	public void turnOff_ObjectiveDestination()
	{
		hasObjectiveDestination = false;
		objectiveDestination = gameObject.transform.position;

		updateMoving();
	}

	void updateMoving()
	{
		if(!navAgent.enabled) return;

		if(inCombat && withinRange)
		{
			navAgent.Stop();
		} else{
			navAgent.Resume();
		}
	}

	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(navAgent.destination.ToString());

		updateMoving();
	}

	public void stopNav()
	{
		navAgent.Stop();
	}

	public bool within_Destination()
	{
		return Vector3.Distance(navAgent.destination, transform.position).AlmostEquals(navAgent.stoppingDistance,1f);
	}

	public void moveTo (Vector3 location, float stopDistance = 0)
	{
		if (location == null) return;

		// If its asking to move to your current destination

		// I have new destination
		navAgent.Stop();
		navAgent.ResetPath();
		navAgent.SetDestination(location);
		navAgent.stoppingDistance = stopDistance;
		navAgent.Resume();
		//moveTo(location);
		
		//movementControler.SimpleMove(direction * speed);
	}
}
