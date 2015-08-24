using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    private GameObject camera;
	// Use this for initialization
	void Start () {

        camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.LookAt(camera.transform.position);
	}
}
