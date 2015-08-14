using UnityEngine;
using System;
using System.Collections.Generic;

/*
 * 	Handles all combat functionality. Location of Health and other combat data
 * 	like attack range and speed.
 * 
 */

public class Combat : MonoBehaviour {

	public float health;
	private float oldHealth;

	public float maxHealth;
	
	public float damage = 0.0F;

	public bool selectable = true;
	public bool dead = false;

    public string targetName;
	public GameObject target;

	public bool isRanged = false;

    public double baseAttackRange;
    public double baseAttackSpeed;
    public double baseHealthRegen;

	// Combat
	private double basicAttackCoolDown = 0;
	
	private Character character;
    public Stats stats;
	
	// Use this for initialization
	void Start () {
		basicAttackCoolDown = 0;
		character = GetComponent<Character>();
        stats = new Stats();
	}

	// Update is called once per frame
	void Update () {

		if(basicAttackCoolDown != 0 && System.DateTime.Now.Millisecond <= 20)
		{
			basicAttackCoolDown--;
		}

		stats.effects.stepTime();
		updateHealth();
		
	}

	/*---Utility Functions----------------------------------------------------------*/

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
		this.health -= amount;
		
		if (this.health <= 0)
		{	
			// deathAnimation();
			// createTimer_to_destory model();

			die();
		}
	}

	// Character takes magic damage
	public void recieve_Damage_Magic(float amount)
	{
        // TODO magicResistance();
		this.health -= amount;
	}

	// Character is healed
	public void recieve_Healing(float amount)
	{
        // TODO healingBuffs();
		this.health += amount;
	}

	// Character causes physical damage (Auto Attack)
	public void cause_Damage_Physical(Combat _target)
	{
		// TODO damageBuffs();
		_target.recieve_Damage_Physical(damage);
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
			
			//giveKillPoints();
			//if(hero) startRespawn_Timer();
			//self.selectable = false;
			this.dead = true;
			this.selectable = false;

			CreepAI test;
			if( test = GetComponent<CreepAI>())
			{
				var objectives = GetComponents<AI_Objective>();
				foreach(var objective in objectives)
				{
					objective.enabled = false;
				}

				test.enabled = false;
			}


			GetComponent<NavMeshAgent>().enabled = false;
			GetComponent<Rigidbody>().useGravity = false;
			character.characterState.addLivingChange(gameObject);

		}

	}

}
