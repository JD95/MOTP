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
                var explosionRange = Physics.OverlapSphere(this.gameObject.transform.position, 5.0f)
                                            .Where(x => TeamLogic.areEnemies(x.gameObject, this.gameObject))
                                            .Select(x => x.GetComponent<Combat>());
                

                foreach (var enemy in explosionRange)
                { enemy.recieve_Damage_Physical(5.0f);}

                GameObject.Destroy(gameObject);
            }
        }

    }

}
