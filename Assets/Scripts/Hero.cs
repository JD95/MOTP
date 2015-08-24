using UnityEngine;
using System;
using System.Collections;

using Utility;

public class Hero : Photon.MonoBehaviour
{
	public string heroTeam;

	private Character character;
	private Combat combatData;
	

	// Navigation
	private Navigation navigation;
	public Waypoint targetLocation;


	void Start ()
	{
		character = GetComponent<Character> ();

		navigation = GetComponent<Navigation>();

		combatData = GetComponent<Combat>();
	}

	// Called every frame
	void Update ()
	{
		adjustDestination();
		combatData.autoAttack();
	}

	// Adjusts location based on new clicks
	void adjustDestination ()
	{
		if (Input.GetButtonDown ("Fire1")) { //Debug.Log ("Click!");	

			Tuple<Vector3, double> clicked =  filterClick(Input.mousePosition);
			navigation.moveTo(clicked.First, clicked.Second);

		}
	
	}

	 // Checks for obstacles in the current path to clicked location 
    Tuple<Vector3, double> filterClick(Vector2 point)
	{
		RaycastHit hit;

		string hitName = name;

        // Raycasts from camera view and ignores the "ignorePlayerClick" layer
		Physics.Raycast(Camera.main.ScreenPointToRay (point), out hit, 100.0F,~(1<<8));

		if (hit.collider.name != transform.name) {

			if(hit.collider.tag.Equals(Utility.TeamLogic.oppositeTeam(heroTeam)))
			{
				combatData.target = hit.collider.gameObject;
				return new Tuple<Vector3, double>(hit.point, combatData.attackRange());

			}else{

                return new Tuple<Vector3, double>(hit.point, 0);
			}
		} else {
            return new Tuple<Vector3, double>(gameObject.transform.position, 0);
		}
	}

}
