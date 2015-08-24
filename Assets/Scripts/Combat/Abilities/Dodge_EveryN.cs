using UnityEngine;
using System.Collections;

using Effect_Management;

public class Dodge_EveryN : Ability{

    public  int dodgeRate;
    private int attackCount = 0;

    public override bool trigger()
    {
        // No triggered effect
        return true;
    }

    public override void passiveEffect()
    {
        caster.GetComponent<Combat>().attackListeners.Add(() => { attackCount++; });

        caster.GetComponent<Combat>().stats.effects.addLastingEffectFor(attribute.DO, new Lasting_Effect<Attribute>(
            "Dodge_EveryN",
            (x, y) => {
                if(dodgeRate == attackCount)
                {
                    Debug.Log("Incite has taken effect!");
                    attackCount = 0;
                    return new Attribute(1.0,0.0,0.0,0.0); // Give 100% chance to dodge
                }
                else
                {
                    return Attribute.zero();
                }
            })
         );
    }
}
