using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Utility;

public class Mine : MonoBehaviour {

    void OnTriggerEnter(Collider unit)
    {
        Combat potential; // Only effects objects with a combat
        if (potential = unit.GetComponent<Combat>())
        { 
            if(TeamLogic.areEnemies(unit.gameObject, this.gameObject))
            {
                var explosionRange = Utility.TeamLogic.enemyCombatsInRange(gameObject, 5.0f);     

                foreach (var enemy in explosionRange)
                { enemy.recieve_Damage_Physical(5.0f);}

                GameObject.Destroy(gameObject);
            }
        }

    }

}
