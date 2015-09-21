using UnityEngine;
using System.Collections;


using AbilityHelp = Utility.AbilityHelp;

public class Cache_Hits : Ability {

    public override bool trigger()
    {
        Vector3 target = AbilityHelp.getTerrain_UnderMouse();

        //var launcher = GetComponentInChildren<Projectile_Launcher>().transform;

        var wave = PhotonNetwork.Instantiate("Projectile_CacheHit", caster.transform.position, caster.transform.rotation, 0).GetComponent<CacheHit_Projectile>();

        wave.target = target;

        wave.caster = caster;

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

