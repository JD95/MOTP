using UnityEngine;
using System.Collections;

public interface AI_Objective{

	bool begin();

	void progress();

	bool end();

}

