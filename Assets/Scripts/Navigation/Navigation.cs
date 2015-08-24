using UnityEngine;
using System.Collections;

/*
 *  This class controls navigation for characters which acts as a middle
 * man for a Game Object's navMeshAgent. Instead of allowing direct access
 * to the navMesh the agent is controled by updateMoving() which has a set
 * of logic determining if the object should be moving or not.
 *  
 *  Different parts of the character control their own bool like "inCombat"
 * which is then passed through the logic defined in updateMoving(). Each
 * time an object changes their bool, the logic in updateMoving() is checked
 * and will stop or resume movement when appropriate.
 * 
 *  This is to prevent different different pieces of code from constantly 
 * swtiching motion on or off at will. Instead it is possible to now detect
 * which parts of code are causing wacky beavhiour in movement as the bools
 * for each are public visible.
 * 
 */
public class Navigation : MonoBehaviour {

    private Character character;

	private float speed;
	private NavMeshAgent navAgent;

    private bool hasObjectiveDestination;
    private Vector3 objectiveDestination;

	public bool inCombat;
    public bool withinRange;

	public void turnOn_inCombat()
	{
		inCombat = true; updateMoving();
	}

	public void turnOff_inCombat()
	{
		inCombat = false; updateMoving();
	}

	public void turnOn_withinRange()
	{
		withinRange = true; updateMoving();
	}

	public void turnOff_withinRange()
	{
		withinRange = false; updateMoving();
	}

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
        character = GetComponent<Character>();
	}

    public void Init()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {

		updateMoving();

        character.setAnimation_State(character.running_State, navAgent.velocity.magnitude > 0);

	}

	public void stopNav()
	{
		navAgent.Stop();
	}

	public bool within_Destination()
	{
		return Vector3.Distance(navAgent.destination, transform.position).AlmostEquals(navAgent.stoppingDistance,1f);
	}

	public void moveTo (Vector3 location, double stopDistance = 0)
	{
		if (location == null) return;

		navAgent.SetDestination(location);
	}
}
