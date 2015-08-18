using UnityEngine;
using System.Collections.Generic;


public class ShallowCopies : Ability {

    const int   abilityLevel = 4;
    const float cloneSpawnDistance = 1.0F;
    const float runDistance = 10F;

    List<GameObject> clones = new List<GameObject>();

    public override bool trigger()
    {
        Vector3 center     = caster.transform.position;
        Vector3 realTarget = runDirection(Input.mousePosition);
        Vector3 firstPoint = Vector3.MoveTowards(center, realTarget, cloneSpawnDistance);

        caster.transform.position = firstPoint;
        caster.transform.LookAt(realTarget);

        clones = makeClonesAt(spawnPositions(abilityLevel, center, firstPoint));

        setDestinations(realTarget);
        
        return true;
    }

    private Vector3 runDirection(Vector2 mousePos)
    {
        RaycastHit hit;

        // Get the point on the terrain where the mouse is
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100.0F, (1 << 10));

        return hit.point;
    }

    private List<transformData> spawnPositions(int level, Vector3 center, Vector3 firstPoint)
    {
        List<transformData> transforms = new List<transformData>();

        for (int i = 1; i < level; i++)
        {
            Vector3 angle = new Vector3(0,i * (360/level),0);
            Vector3 nextPosition = VectorHelp.RotatePointAroundPivot(firstPoint, center, angle);
            transforms.Add(new transformData(nextPosition, angle));
        }

        return transforms;
    }

    private List<GameObject> makeClonesAt(List<transformData> tDatas)
    {
        List<GameObject> clones = new List<GameObject>();

        foreach (var tData in tDatas)
        {
            var clone = PhotonNetwork.Instantiate("HeroPrefabA", tData.position, Quaternion.Euler(tData.rotation), 0);
            clone.transform.eulerAngles += caster.transform.eulerAngles;

            clone.GetComponent<CharacterController>().enabled = true;
            clone.GetComponent<Navigation>().Init();

            clones.Add(clone);
        }

        return clones;
    }

    private void setDestinations(Vector3 realTarget)
    {
        caster.GetComponent<Navigation>().moveTo(realTarget);

        foreach(var clone in clones)
        {
            clone.GetComponent<Navigation>().moveTo(clone.transform.forward * runDistance);
        }
    }

    public override void passiveEffect()
    {
        // Do Nothing
    }
}
