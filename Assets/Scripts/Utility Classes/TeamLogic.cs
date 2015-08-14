using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Utility
{
    public class TeamLogic
    {
        public const string TeamA = "teamA";
        public const string TeamB = "teamB";

        public static string oppositeTeam(string thisPlayer)
        {
            if (thisPlayer == TeamA) return TeamB;
            else return TeamA;
        }

        public static bool areEnemies(GameObject a, GameObject b)
        {
            // Is b on the opposite team of a?
           return a.CompareTag(Utility.TeamLogic.oppositeTeam(b.tag));
        }

        public static List<GameObject> enemyObjsInRange(GameObject unit,float radius)
        {
            return Physics.OverlapSphere(unit.transform.position, 5.0f)
                          .Where(x => TeamLogic.areEnemies(x.gameObject, unit))
                          .Select(x => x.gameObject).ToList();
        }

        public static List<Combat> enemyCombatsInRange(GameObject unit, float radius)
        {
            return enemyObjsInRange(unit, radius)
                        .Select(x => x.GetComponent<Combat>())
                        .ToList();
        }

    }
}