using UnityEngine;
using System;
using System.Collections.Generic;

using CharacterState_Effects = Effect_Management.CharacterState_Effects;

/*
 * 	Handles all combat functionality. Location of Health and other combat data
 * 	like attack range and speed.
 * 
 */
public delegate void attackListener();

public class Combat : MonoBehaviour {

    private static System.Random random = new System.Random();

    public bool hero;

	public float health;
	private float oldHealth;

	public float maxHealth;

    public float mana;
	
	public float damage = 0.0F;

	public bool selectable = true;
	public bool dead = false;

    public string targetName;
	public GameObject target;

	public bool isRanged = false;

    // Base Stats
    public double baseAttackRange;
    public double baseAttackSpeed;
    public double baseHealthRegen;
    public double baseDodgeRate;
    public double baseManaRegen;
    public double baseMaxMana;

	// Clocks
	private double basicAttackCoolDown = 0;
    private double regenClock = 0;
	
	private Character character;
    public Stats stats;

    public List<attackListener> attackListeners = new List<attackListener>();

    void onAttacked()
    {
        foreach (var listener in attackListeners)
        {
            listener();
        }
    }

	// Use this for initialization
	void Start () {
		basicAttackCoolDown = 0;
		character = GetComponent<Character>();
        stats = new Stats();
	}

	// Update is called once per frame
	void Update () {

        autoAttackCD();

        if (hero) { regen(); }

		stats.effects.stepTime();
		updateHealth();
		
	}

    void autoAttackCD()
    {
        if (basicAttackCoolDown > 0)
        {
            basicAttackCoolDown -= Time.deltaTime * 1;
        }
    }

	void regen()
    {

        var _maxMana = maxMana(); // Calculate this here to avoid having to call function twice

        if (regenClock > 0) { regenClock -= Time.deltaTime * 1; return; }

        if(health < maxHealth) // Regen Health
        {
            var regen = (float)stats.effects.getChangesFor(attribute.HPReg).applyTo(baseHealthRegen);
            health += regen;
        } else if (health > maxHealth){ health = maxHealth; }

        if (mana < _maxMana) // Regen Mana
        {
            var regen = (float)stats.effects.getChangesFor(attribute.MPReg).applyTo(baseManaRegen);
            mana += regen;
        } else if (mana > _maxMana) { mana =_maxMana; }

        regenClock = 5.0;
    }

	public bool beenDamaged()
	{
		return oldHealth > health;
	}

	public void updateHealth()
	{
        health = (float)stats.effects.getChangesFor(attribute.HP).applyTo(health);

        if(health <= 0)
        {
            die();
        }

		oldHealth = health;
	}

	public float healthPercent()
	{
		return health / maxHealth;
	}
	
	public double attackSpeed()
	{
        return stats.effects.getChangesFor(attribute.AS).applyTo(baseAttackSpeed);
	}

    public double attackRange()
    {
        return stats.effects.getChangesFor(attribute.AR).applyTo(baseAttackRange);
    }

    public float attackDamage()
    {
        return (float) stats.effects.getChangesFor(attribute.AD).applyTo(damage);
    }

    public float dodgeRate()
    {
        return (float)stats.effects.getChangesFor(attribute.DO).applyTo(baseDodgeRate);
    }

    public float maxMana()
    {
        return (float)stats.effects.getChangesFor(attribute.MaxMP).applyTo(baseMaxMana);
    }

    public int level()
    {
        return stats.level;
    }

	public bool targetWithin_AttackRange()
	{
		if (target.transform != null && transform != null)
			return Vector3.Distance(target.transform.position, transform.position) <= baseAttackRange;
		else return false;
	}

	// Character takes physical damage
	public void recieve_Damage_Physical(float amount)
	{
        onAttacked();

        double chanceToHit   = random.NextDouble();
        double chanceToDodge = dodgeRate();

        if (chanceToHit > chanceToDodge)
        {
            this.health -= (float)stats.effects.getChangesFor(attribute.ARM).applyTo(amount);

            if (this.health <= 0) { die(); }
        }
	}

	// Character takes magic damage
	public void recieve_Damage_Magic(float amount)
	{
        onAttacked();

		this.health += (float) stats.effects.getChangesFor(attribute.MR).applyTo(amount);
	}

	// Character is healed
	public void recieve_Healing(float amount)
	{
        if (health + amount <= maxHealth)
        {
            this.health += amount;
        }
        else
        {
            this.health = maxHealth;
        }
	}

	// Character causes physical damage (Auto Attack)
	public void cause_Damage_Physical(Combat _target)
	{
		_target.recieve_Damage_Physical(attackDamage());
        
        if (_target.dead) stats.killScore++;
	}

	public void autoAttack()
	{
		if(target != null && targetWithin_AttackRange()){

			transform.LookAt(target.transform);

			if(basicAttackCoolDown <= 0)
			{
				if(isRanged)
				{
					GetComponentInChildren<Projectile_Launcher>().fire(target);
					basicAttackCoolDown = attackSpeed();
				}else{

					cause_Damage_Physical(target.GetComponent<Combat>());
				}

				basicAttackCoolDown = attackSpeed();
			}
		}

	}

	private void die()
	{
		if(/*prevent_Death()*/ false)
		{
			// do something...

		}else{

			//broadcastDeath();
			//stopMovement();
			//deathAnimation();
			if(/*reverse_Death()*/ false)
			{
				//revive();
			}
			
			//if(hero) startRespawn_Timer();

            character.setAnimation_State(character.dead_State, true);
			this.dead = true;
			this.selectable = false;


			GetComponent<NavMeshAgent>().enabled = false;
			GetComponent<Rigidbody>().useGravity = false;

            if(hero)
            {
                var respawnFunc = CharacterState_Effects.respawnHero(gameObject);
                character.characterState.addTimedEffect(respawnFunc);
            }
            else
            {

                CreepAI test;
                if (test = GetComponent<CreepAI>())
                {
                    var objectives = GetComponents<AI_Objective>();
                    foreach (var objective in objectives)
                    {
                        objective.enabled = false;
                    }

                    test.enabled = false;
                }

                var removeBodyFunc = CharacterState_Effects.destroyCharacterObject(gameObject);
                character.characterState.addTimedEffect(removeBodyFunc);
            }

		}

	}

}
