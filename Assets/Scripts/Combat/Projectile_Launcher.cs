using UnityEngine;
using System.Collections;

public class Projectile_Launcher : MonoBehaviour {

	public string default_Projectile = "Projectile_Creep_Auto";

	// Use this for initialization
	public void fire (GameObject _target) {

        fire(_target, default_Projectile);
	}

    public void fire(GameObject _target, string missle)
    {

        if (_target == null) return;

        GameObject projClone = PhotonNetwork.Instantiate(missle, transform.position, transform.rotation, 0);

        Debug.Log(projClone.name + " is being shot at!");

        projClone.GetComponent<Projectile>().target = _target;
    }

}
