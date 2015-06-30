using UnityEngine;
using System.Collections;

interface AI_Objective
{
	// A test for whether the objective should begin
	bool begin();

	// The steps to carry out the objective
	void progress();

	// How to tell when the object is completed
	bool end();
}