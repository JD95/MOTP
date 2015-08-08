using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Target target;
	public float damage;
	public float speed;

	public Projectile(Target _target){

		target = _target;
	}

	void Update(){

		if(target.location == null) 
		{
			GameObject.Destroy(gameObject); return;
		}
		if(transform.position != target.location.position)
		{
			transform.position = Vector3.Lerp(transform.position, target.location.position, (speed / 60.0F)); 
		}
	}

	void OnTriggerEnter(Collider hit){

		Combat targetHit;

		//Debug.Log(hit.name);
		//Debug.Log("I hit something!!");

		if (targetHit = hit.GetComponent<Combat>()){

			if (targetHit.self == target)
			{
				targetHit.recieve_Damage_Physical(damage, ref target);
				GameObject.Destroy(gameObject);
			}
		}
	}
	
}
