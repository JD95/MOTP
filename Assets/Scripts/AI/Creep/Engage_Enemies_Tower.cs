using UnityEngine;
using System.Collections;

public class Engage_Enemies_Tower : Engage_Enemies {

	protected override void handle_OutofRange()
	{
		Debug.Log (	"I am a tower and my target is out of range!");
	}
}
