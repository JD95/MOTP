using UnityEngine;
using System.Collections;

using AbilityHelp = Utility.AbilityHelp;
using TeamLogic = Utility.TeamLogic;

public class Valgrind : Ability {

	public override bool trigger()
    {
        GameObject target = AbilityHelp.getSelectable_UnderMouse();

        if (TeamLogic.areAllies(caster, target))
        {
            target.GetComponent<Combat>().stats.effects.removeHarmfulEffects();
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public override void passiveEffect()
    {
 	//throw new System.NotImplementedException();
    }

    public override void registerEffects()
    {
        //throw new System.NotImplementedException();
    }
}
