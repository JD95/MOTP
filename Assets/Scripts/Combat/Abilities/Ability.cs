using UnityEngine;
using System.Collections;

public abstract class Ability  : Photon.MonoBehaviour
{
    protected GameObject caster;

    public string   spawnName;
    public float    cooldownTime;

    public abstract void registerEffects();
    public abstract bool trigger();
    public abstract void passiveEffect();

    public void setCaster(GameObject _caster)
    {
        caster = _caster;
    }

}

/*
 * Ability filters are a means by which certain
 * moves can change what other moves are available
 * 
 * When an ability filter is run, it will return either
 * 
 * true: continue to use the filter
 * 
 *      or
 *      
 * false: do not use this filter anymore and return to default
 * 
 */ 
public delegate bool AbilityFilter();

public interface hasOverride
{
    AbilityFilter abilityOverride();
}