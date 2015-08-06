using UnityEngine;
using System.Collections;

public class Hero : Photon.MonoBehaviour
{
	const string TeamA = "teamA";
	const string TeamB = "teamB";

	public string heroTeam;

	private Character character;
	private Combat combatData;
	

	// Navigation
	private NavMeshAgent navigation;
	public Waypoint targetLocation;


	/*-- Main Functions ---------------------------------------------------------------------------*/
	void Start ()
	{
		character = GetComponent<Character> ();

		targetLocation = GameObject.Find("A").GetComponent<Waypoint>();

		navigation = GetComponent<NavMeshAgent>();
		navigation.stoppingDistance = 0;

		combatData = GetComponent<Combat>();
	}

	// Called every frame
	void Update ()
	{
		adjustDestination();
		combatData.autoAttack();
	}

	/*-- Movement ---------------------------------------------------------------------------*/

	/*
	 * 	Adjusts location based on new clicks
	 */
	void adjustDestination ()
	{
		if (Input.GetButtonDown ("Fire1")) { //Debug.Log ("Click!");	

			navigation.destination = filterClick(Input.mousePosition);
			character.currentAnimation = Animations.run;
			navigation.Resume();

		}
	
	}


	/*
	 *	Checks for obstacles in the current path to
	 *	clicked location
	 */
	Vector3 filterClick (Vector2 point)
	{
		RaycastHit hit;
		Vector3 click = new Vector3();
		string hitName = name;

		if(Physics.Raycast(Camera.main.ScreenPointToRay (point), out hit, 100.0F,~(1<<8)))
		{
			Debug.Log (hit.collider.name);

			if (hit.collider.name != transform.name) {

				//Debug.Log("Not me!");

				if(hit.collider.tag.Equals(oppositeTeam(heroTeam)))
				{
					//Debug.Log("Click detection sucessful!");

					combatData.target = hit.collider.GetComponent<Combat>().self;
					navigation.stoppingDistance = combatData.attackRange;

					return hit.point;

				}else{
					// You are travelling to a location on terrain
					navigation.stoppingDistance = 0;
					return  hit.point; //hitName = hit.collider.name;
				}

			}
		}

		return point;
	}

	
	public string oppositeTeam(string thisPlayer)
	{
		if(thisPlayer == TeamA)return TeamB;
		else return TeamA;
	}

	/*-- Tests ---------------------------------------------------------------------------*/
	void testFollowPath()
	{

		if (transform.position.AlmostEquals(targetLocation.getPosition(), 0.1F))
			targetLocation = targetLocation.next;

		transform.LookAt(targetLocation.transform.position);
	}
}
