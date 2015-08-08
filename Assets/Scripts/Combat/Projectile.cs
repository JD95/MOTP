using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public GameObject target;
	public float damage;
	public float speed;

	public Projectile(GameObject _target){

		target = _target;
	}

	void Update(){

		if(target == null) 
		{
			GameObject.Destroy(gameObject); return;
		}
		if(transform.position != target.transform.position)
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position, (speed / 60.0F)); 
		}
	}

	void OnTriggerEnter(Collider hit){

		//Debug.Log(hit.name);
		//Debug.Log("I hit something!!");

		if (hit.gameObject == target)
		{
			hit.gameObject.GetComponent<Combat>().recieve_Damage_Physical(damage);
			GameObject.Destroy(gameObject);
		}

	}
	
}
