using UnityEngine;
using System;
using System.Collections;

using AbilityHelp = Utility.AbilityHelp;
using TeamLogic = Utility.TeamLogic;

using Effect_Management;

public class MemoryLeak : Ability {

    public override bool trigger()
    {
        
        // Select target
        GameObject target = AbilityHelp.getSelectable_UnderMouse();

        // Check that they are enemy
        if (target == null) return false;

        if (!TeamLogic.areEnemies(target, caster)) return false;

        // Apply debuff
        target.GetComponent<Combat>().stats.effects.addTimedEffectFor(attribute.MaxMP, new Timed_Effect<Effect_Management.Attribute>(
            new effectInfo("Memory Leak", EffectType.Posion, 1, 10.0, DateTime.Now),
            Attribute_Effects.changeBy_percent(-0.50),
            () => {}));

        
        return true;
    }

    public override void passiveEffect()
    {
        // None
    }
}
