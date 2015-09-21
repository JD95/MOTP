using UnityEngine;
using System;
using System.Collections;

using Effect_Management;
using Attribute = Effect_Management.Attribute;



public class Dodge_EveryN : Ability{

    const string thisAbility = "Dodge_EveryN";

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

        caster.GetComponent<Combat>().stats.effects.addLastingEffectFor(attribute.DO, thisAbility);
    }

    // Because Dodge_EveryN needs references to dodgeRate and attackCount, we can't make it a static function
    // Since we can only pass static functions into our table, we just have to make this function return a
    // factory that creates new lasting effects
    public Func<Lasting_Effect<Attribute>> makeDodge_EveryN ()
    {
        return () => new Lasting_Effect<Attribute>(
                    thisAbility,
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
                    }
                );
    }

    public override void registerEffects()
    {
        // Add the ability to the list
        Debug.Log("Dodge_EveryN added to dictionary");
        Effect_Management.Attribute_Manager.lastingEffects.Add(thisAbility, makeDodge_EveryN());
    }
}
