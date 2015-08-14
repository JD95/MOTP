using UnityEngine;
using System.Collections;

public abstract class Ability  : Photon.MonoBehaviour
{
    protected GameObject caster;

    public string   spawnName;
    public float    cooldownTime;

    public abstract bool trigger();
    public abstract void passiveEffect();

    public void setCaster(GameObject _caster)
    {
        caster = _caster;
    }

}
