using UnityEngine;
using System.Linq;
using System.Collections.Generic;


using TeamLogic = Utility.TeamLogic;

public class CacheHit_Projectile : MonoBehaviour {

	public Vector3 target;
    public GameObject caster;
	public float heal;
	public float speed;

    public List<GameObject> allies = new List<GameObject>();

    public CacheHit_Projectile(GameObject caster, Vector3 _target)
    {
		target = _target;
	}

	void Update(){

		if(transform.position != target)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); 
		}
        else
		{
            foreach (var ally in allies.Where(x => x != null).Select(x=>x.GetComponent<Combat>()))
            {
                if(ally != null)
                {
                    ally.recieve_Healing(heal * allies.Count);
                }
            }

			GameObject.Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider hit){
		
        if (TeamLogic.areAllies(caster, hit.gameObject))
        {
            allies.Add(hit.gameObject);
        }

	}
}
