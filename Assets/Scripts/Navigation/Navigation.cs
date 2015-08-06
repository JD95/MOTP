using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	private float speed;
	private NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(navAgent.destination.ToString());
	}

	public void stopNav()
	{
		navAgent.Stop();
	}

	public bool within_Destination()
	{
		return Vector3.Distance(navAgent.destination, transform.position).AlmostEquals(navAgent.stoppingDistance,1f);
	}

	public void moveTo (Transform location)
	{
		if (location == null) return;

		// If its asking to move to your current destination

		// I have new destination
		navAgent.Stop();
		navAgent.ResetPath();
		navAgent.SetDestination(location.position);
		navAgent.Resume();
		//moveTo(location);
		
		//movementControler.SimpleMove(direction * speed);
	}
}
