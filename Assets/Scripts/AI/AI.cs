using UnityEngine;
using System.Collections.Generic;

public class AI : MonoBehaviour {


	Stack<AI_Objective> active_Objectives;
	public MonoBehaviour[] secondary_Objectives;
	public MonoBehaviour main_Objective;

	// Use this for initialization
	void Start () {
	
		active_Objectives.Push(main_Objective.GetComponent<AI_Objective>());
	}
	
	// Update is called once per frame
	void Update () {
		active_Objectives.Peek().progress();
	}
}
