using UnityEngine;
using System.Collections;

public class Projectile_Launcher : MonoBehaviour {

	public Rigidbody projectile;

	// Use this for initialization
	public void fire (GameObject _target) {
	
		Rigidbody projClone = (Rigidbody) Instantiate(projectile, transform.position, transform.rotation);

		projClone.GetComponent<Projectile>().target = _target;
	}

}
