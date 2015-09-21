using UnityEngine;
using System.Collections;

public class DNS : Ability {

	public override bool trigger()
    {
        var targetLocation = Utility.AbilityHelp.getTerrain_UnderMouse();

        // if area is visible

        caster.transform.position = targetLocation;
        caster.GetComponent<Navigation>().moveTo(targetLocation);

        return true;
    }

    public override void passiveEffect()
    {
        // None
    }

    public override void registerEffects()
    {
        //throw new System.NotImplementedException();
    }
}
