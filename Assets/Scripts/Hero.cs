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

			navigation.destination = checkForObstacles(Input.mousePosition);
			character.currentAnimation = Animations.walking;

		}

		// If you have arrived at destination, play idle animation
		if(within_Destination())
			character.currentAnimation = Animations.idle;
	}

	bool within_Destination()
	{
		return Vector3.Distance(navigation.destination, transform.position).AlmostEquals(navigation.stoppingDistance,1f);
	}
	
	/*
	 *	Checks for obstacles in the current path to
	 *	clicked location
	 */
	Vector3 checkForObstacles (Vector2 point)
	{
		RaycastHit hit = new RaycastHit ();
		Vector3 click = new Vector3();
		string hitName = name;

		// If player clicks in the middle of a mountain, if will make the character
		// Walk towards that area until it reaches the base of the mountain
		if (Physics.Raycast (Camera.main.ScreenPointToRay (point), out hit, 100.0f)) {
			Debug.Log(hit.collider.name);
			if (hit.collider.name != transform.name) {

				if(hit.collider.CompareTag(oppositeTeam(heroTeam)))
				{
					Debug.Log("Click detection sucessful!");

					combatData.target = hit.transform;
					navigation.stoppingDistance = combatData.attackRange;

					return hit.point;

				}else{

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
