using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Waypoint : MonoBehaviour {

	public Waypoint next;

	public Waypoint(Vector3 position)
	{
		transform.position = position;
	}

	public Vector3 getPosition()
	{
		return transform.position;
	}

}


