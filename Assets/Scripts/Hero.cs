using UnityEngine;
using System;
using System.Collections;

public class Hero : Photon.MonoBehaviour
{
	const string TeamA = "teamA";
	const string TeamB = "teamB";

	public string heroTeam;

	private Character character;
	private Combat combatData;
	

	// Navigation
	private Navigation navigation;
	public Waypoint targetLocation;


	/*-- Main Functions ---------------------------------------------------------------------------*/
	void Start ()
	{
		character = GetComponent<Character> ();

		targetLocation = GameObject.Find("A").GetComponent<Waypoint>();

		navigation = GetComponent<Navigation>();

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

			Tuple<Vector3, float> clicked =  filterClick(Input.mousePosition);
			navigation.moveTo(clicked.First, clicked.Second);

		}
	
	}


	/*
	 *	Checks for obstacles in the current path to
	 *	clicked location
	 */

	Tuple<Vector3,float> filterClick (Vector2 point)
	{
		RaycastHit hit;
		Vector3 click = new Vector3();
		string hitName = name;

		Physics.Raycast(Camera.main.ScreenPointToRay (point), out hit, 100.0F,~(1<<8));

		//Debug.Log (hit.collider.name);

		if (hit.collider.name != transform.name) {

			if(hit.collider.tag.Equals(oppositeTeam(heroTeam)))
			{
				combatData.target = hit.collider.GetComponent<Combat>().self;
				return new Tuple<Vector3, float>(hit.point, combatData.attackRange);

			}else{

				return  new Tuple<Vector3, float>(hit.point, 0); //hitName = hit.collider.name;
			}
		} else {
			return new Tuple<Vector3, float>(gameObject.transform.position, 0);
		}
	}
	
	public string oppositeTeam(string thisPlayer)
	{
		if(thisPlayer == TeamA)return TeamB;
		else return TeamA;
	}

}
