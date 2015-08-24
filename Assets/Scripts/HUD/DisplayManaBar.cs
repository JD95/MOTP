using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayManaBar : MonoBehaviour {

    private Combat player_combatData;
    private Slider mana;

    void Start()
    {
        player_combatData = GetComponentInParent<Combat>();
        mana = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        mana.maxValue = (float)player_combatData.baseMaxMana;
        mana.value = player_combatData.mana;
    }
}
