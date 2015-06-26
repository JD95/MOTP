using UnityEngine;
using System.Collections;

public class Hero : Photon.MonoBehaviour
{
	bool clicked = false;

	public bool mine = false;

	public PhotonPlayer theOwner;

	private Character character;

	public Waypoint targetLocation;
	
	private float gunTime = 0;

	public Transform ShotPrefab;
	
	void Start ()
	{
		character = GetComponent<Character> ();
		targetLocation = GameObject.Find("A").GetComponent<Waypoint>();
	}
	
	void Awake ()
	{
		if (PhotonNetwork.isMasterClient) {
		}
	}

	[RPC]
	void SetPlayer (PhotonPlayer player)
	{
		theOwner = player;
		if (player == PhotonNetwork.player)
			enabled = true;
	}
	
	
	void Update ()
	{
		Camera.main.GetComponent<CameraControl> ().SetTarget (transform);

		adjustDestination();	
	
		//testFollowPath();

		if(targetLocation != null)
			coverDistance (transform.position ,targetLocation.getPosition());

//			gunTime -= Time.deltaTime;
//			if (target == null) {
//				float distance = (serverCurrentClick - transform.position).magnitude;
//				if (serverCurrentClick != Vector3.zero && distance > 1) {
//					transform.LookAt (serverCurrentClick);
//					Vector3 euler = transform.localEulerAngles;
//					euler.x = 0;
//					euler.z = 0;
//					transform.localEulerAngles = euler;
//					movement = transform.TransformDirection (Vector3.forward) * 5 - Vector3.up * 10;
//				}
//				else {
//					movement = Vector3.zero - Vector3.up * 10;
//				}
//			} else {
//				transform.LookAt (target.transform);
//				float distance = (transform.position - target.transform.position).magnitude;
//				if (distance < c.range) {
//					movement = Vector3.zero - Vector3.up * 10;
//					if (clicked) {
//						clicked = false;
//						if (gunTime <= 0) {
//							gunTime = 0.25f;
//							if (target.health > 0) {
//								target.Hit(c.Damage());
//								PhotonNetwork.Instantiate ("ShotPrefab", transform.position, transform.rotation, 0);
//								if (target.health <= 0 && target.tag != tag) {
//									if (target.isHero) {
//										c.Xp(3);
//									}
//									else {
//										c.creeps++;
//										c.Xp(1);
//									}
//								}
//							}
//						}
//					}
//				} else {
//					movement = transform.TransformDirection (Vector3.forward) * 5 - Vector3.up * 10;
//				}
//			}
//			
//			if (c.health <= 0) {
//				c.health = c.maxHealth;
//				transform.position = originalPos;
//			}
//			else {
//				c.Hit(-0.25f * Time.deltaTime);
//			}
		
	}

	void adjustDestination ()
	{
		if (Input.GetButtonDown ("Fire1")) { //Debug.Log ("Click!");	

			// Remove old waypoint
			//GameObject.Destroy(targetLocation);

			// Create a new waypoint on your client only
			Waypoint clickedLocation = ((GameObject)Resources.Load("WayPoint")).GetComponent<Waypoint>();

			// CURRENTLY NOT RETURNING hitName!
			clickedLocation.transform.position = checkForObstacles(Input.mousePosition);

			targetLocation = clickedLocation;
			
			// Moves Character
			transform.LookAt(targetLocation.transform.position);

		}
	}

	void coverDistance(Vector3 currPosition, Vector3 destination)
	{
		if (Vector3.Distance(currPosition,destination) >= 0.1){

			character.doWalkAnimation();

			character.moveTo(targetLocation);

		} else {
			character.doIdleAnimation();
		}
	}

	Vector3 checkForObstacles (Vector2 point)
	{
		RaycastHit hit = new RaycastHit ();
		Vector3 click = new Vector3();
		string hitName = name;

		// If player clicks in the middle of a mountain, if will make the character
		// Walk towards that area until it reaches the base of the mountain
		if (Physics.Raycast (Camera.main.ScreenPointToRay (point), out hit, 100.0f)) {
			if (hit.collider.name != transform.name) {
				return  hit.point; //hitName = hit.collider.name;
			}
		}
		return point;
	}

	void testFollowPath()
	{

		if (transform.position.AlmostEquals(targetLocation.getPosition(), 0.1F))
			targetLocation = targetLocation.next;

		transform.LookAt(targetLocation.transform.position);
	}

	/*
	[RPC]
	void SendMovementInput (float x, float y, float z, string hitName)
	{
		if (hitName != "Terrain") {
			GameObject go = GameObject.Find (hitName);
			if (go != null) {
				target = go.GetComponent<Character> ();
				
			}
		} else {
			target = null;
		}
		serverCurrentClick = new Vector3 (x, y, z);
		clicked = true;
	}
	*/

	/*
	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			Vector3 pos = transform.position;
			float rot = transform.eulerAngles.y;
			Vector3 vel = movement;
			float health = c.health;
			int id = c.charID;
			int kills = Character.killsB;
			if (tag == "teamA")
				kills = Character.killsA;
			int level = c.level;
			char v = Character.victoriousTeam;
			stream.Serialize (ref v);
			stream.Serialize (ref id);
			stream.Serialize (ref kills);
			stream.Serialize (ref level);
			stream.Serialize (ref pos);
			stream.Serialize (ref rot);
			stream.Serialize (ref vel);
			stream.Serialize (ref health);
		} else {
			Vector3 posReceive = Vector3.zero;
			float rotReceive = 0;
			Vector3 velReceive = Vector3.zero;
			float health = 0;
			int id = 0;
			int kills = 0;
			int level = 0;
			char v = '0';
			stream.Serialize (ref v);
			stream.Serialize (ref id);
			stream.Serialize (ref kills);
			stream.Serialize (ref level);
			stream.Serialize (ref posReceive);
			stream.Serialize (ref rotReceive);
			stream.Serialize (ref velReceive);
			stream.Serialize (ref health);
			Character.victoriousTeam = v;
			transform.position = posReceive;
			Vector3 rot = transform.eulerAngles;
			rot.y = rotReceive;
			transform.eulerAngles = rot;
			movement = velReceive;
			c.health = health;
			c.charID = id;
			if (tag == "teamA")
				Character.killsA = kills;
			else
				Character.killsB = kills;
			c.level = level;
		}
	}
	*/
}
