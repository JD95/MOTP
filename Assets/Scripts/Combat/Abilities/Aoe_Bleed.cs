using UnityEngine;
using System.Collections;

using Utility;

public class Aoe_Bleed : Ability {

    public float range;

    public override bool trigger()
    {
        var toSlice = TeamLogic.enemyCombatsInRange(caster, range);

        foreach(var enemy in toSlice)
        {
            enemy.recieve_Damage_Physical(5.0f);
            //enemy.
        }

        return true;
    }

    public override void passiveEffect()
    {
        
    }

}
