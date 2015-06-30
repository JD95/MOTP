using UnityEngine;
using System.Collections;

public class Hero : Photon.MonoBehaviour
{
	const string TeamA = "teamA";
	const string TeamB = "teamB";

	public string heroTeam;

	private Character character;
	public Transform target;

	// Combat
	public float attackDistance;

	// Navigation
	private NavMeshAgent navigation;
	public Waypoint targetLocation;


	/*-- Main Functions ---------------------------------------------------------------------------*/
	void Start ()
	{
		character = GetComponent<Character> ();
		targetLocation = GameObject.Find("A").GetComponent<Waypoint>();
		navigation = GetComponent<NavMeshAgent>();
	}
	
	void Awake ()
	{}

	// Called every frame
	void Update ()
	{
		adjustDestination();
		autoAttack();
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

		if(navigation.destination == transform.position)
			character.currentAnimation = Animations.idle;
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

					target = hit.transform;
				}

				return  hit.point; //hitName = hit.collider.name;
			}
		}
		return point;
	}

	void autoAttack()
	{
		if(target != null && Vector3.Distance(target.position, transform.position) <= attackDistance)
		{
			character.cause_Damage_Physical(target.GetComponent<Character>());
		}
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
