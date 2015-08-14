using UnityEngine;
using System.Collections;

public class Stats 
{

    public int level { get; set; }
    public int creepScore { get; set; }
    public int killScore { get; set; }
    public int assists { get; set; }

    public Effect_Management.Attribute_Manager effects;

    public Stats()
    {
        effects = new Effect_Management.Attribute_Manager();
        level = 1;
        creepScore = 0;
        killScore = 0;
        assists = 0;
    }
}
