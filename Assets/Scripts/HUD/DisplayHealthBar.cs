using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayHealthBar : MonoBehaviour {

    private Combat player_combatData;
    private Slider health;

    void Start()
    {
        player_combatData = GetComponentInParent<Combat>();
        health = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        health.maxValue = (float)player_combatData.maxHealth;
        health.value = player_combatData.health;
    }
}
