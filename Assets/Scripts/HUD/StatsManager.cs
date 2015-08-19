using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsManager : MonoBehaviour {

    public GameObject playerHealth;
    public GameObject playerMana;

    private Combat player_combatData;
    private Slider health;
    private Slider mana;

    private bool init = false;

	// Use this for initialization
	public void Init () {
        player_combatData = GameObject.Find("player").GetComponent<Combat>();
        health = playerHealth.GetComponent<Slider>();
        mana = playerMana.GetComponent<Slider>();
        init = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (init)
        {
            health.maxValue = player_combatData.maxHealth;
            mana.maxValue = (float)player_combatData.baseMaxMana;

            health.value = player_combatData.health;
            mana.value = player_combatData.mana;
        }
	}
}
