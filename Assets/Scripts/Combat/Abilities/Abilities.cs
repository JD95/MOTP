using UnityEngine;
using System.Collections;

/*
 *  We need the game objects to make constructing champs and
 *  their abilities simpler.
 * 
 */ 
public class Abilities : MonoBehaviour {

    private Combat combatData;

    private AbilityFilter inputFilter;

    private Ability q_Slot;
    private Ability w_Slot;
    private Ability e_Slot;
    private Ability r_Slot;

    // Q ABILITY
    public GameObject q;
    public int      q_Level;
    public bool     q_UsePassive;
    public float[]  q_ResourceCost;

    // W ABILITY
    public GameObject w;
    public int      w_Level;
    public bool     w_UsePassive;
    public float[]  w_ResourceCost;

    // E ABILITY
    public GameObject e;
    public int      e_Level;
    public bool     e_UsePassive;
    public float[]  e_ResourceCost;

    // R ABILITY
    public GameObject r;
    public int      r_Level;
    public bool     r_UsePassive;
    public float[]  r_ResourceCost;

    void Start()
    {
        combatData = GetComponent<Combat>();

        initAbility(ref q_Slot, q);
        initAbility(ref w_Slot, w);
        initAbility(ref e_Slot, e);
        initAbility(ref r_Slot, r);

        applyPassives();

        inputFilter = defaultFilter;
    }

    void initAbility(ref Ability ability, GameObject component)
    {
        ability = component.GetComponent<Ability>();
        ability.setCaster(gameObject);
        ability.registerEffects();
    }

    void Update()
    {

        if (inputFilter == null || inputFilter() == false)
        {
            inputFilter = defaultFilter;
        }
        
    }

    bool defaultFilter()
    {
        if (Input.GetKeyDown("q"))
        { 
            if(useAbility(q_Slot, q_Level, q_ResourceCost)) 
                inputFilter = returnOverride(q_Slot); 
        }
        else if (Input.GetKeyDown("w"))
        { 
            if(useAbility(w_Slot, w_Level, w_ResourceCost))
                inputFilter = returnOverride(w_Slot);

            Debug.Log("w was pressed!");
        }
        else if (Input.GetKeyDown("e"))
        { 
            if(useAbility(e_Slot, e_Level, e_ResourceCost))
                inputFilter = returnOverride(e_Slot);
        }
        else if (Input.GetKeyDown("r"))
        { 
            if(useAbility(r_Slot, r_Level, r_ResourceCost))
                inputFilter = returnOverride(r_Slot);
        }

        return true;
    }

    void applyPassives()
    {
        if (q_UsePassive) q_Slot.passiveEffect();
        if (w_UsePassive) w_Slot.passiveEffect();
        if (e_UsePassive) e_Slot.passiveEffect();
        if (r_UsePassive) r_Slot.passiveEffect();
    }

    AbilityFilter returnOverride(Ability _slot)
    {
        var slot = _slot as hasOverride;

        if(slot != null)
        {
            return slot.abilityOverride();
        }
        else
        {
            return defaultFilter;
        }
    }


    // Returns a bool if the ability was actually triggered or not
    // eg. if you have a target spell, but did not target anything
    bool useAbility(Ability ability, int level, float[] resourceCost)
    {
        bool haveEnoughMana = combatData.mana - resourceCost[level] >= 0;

        if (haveEnoughMana && ability.trigger())
        {
            consumeResource(resourceCost[level]);
            return true;
        }
        else
        {
            return false; // Ability was not used
        }
    }

    void consumeResource(float cost)
    {
        combatData.mana -= cost;
    }

}
