using UnityEngine;
using System.Collections;

using Utility;

public class CreateObject : Ability {

    public override bool trigger()
    {
        //Create the object in the scene
        var obj = PhotonNetwork.Instantiate(spawnName, caster.transform.position, Quaternion.identity, 0);
        obj.tag = caster.tag;

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
