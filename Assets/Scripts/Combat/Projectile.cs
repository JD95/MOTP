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
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime); 
		}
	}

	void OnTriggerEnter(Collider hit){

		if (hit.gameObject == target)
		{
			hit.gameObject.GetComponent<Combat>().recieve_Damage_Physical(damage);
			GameObject.Destroy(gameObject);
		}

	}
	
}
