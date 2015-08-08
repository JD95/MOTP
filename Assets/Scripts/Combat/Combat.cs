﻿using UnityEngine;
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

	public string targetName;

	public float maxHealth;
	
	public int creeps = 0;
	public int level = 1;
	
	//public float range = 2;
	public float damage = 0.0F;

	public Target self;
	public Target target;
	public List<Target> targetingMe;

	public bool isRanged = false;

	// Combat
	private float basicAttackCoolDown = 0;

	public float attackRange;
	public float baseAttackSpeed;
	
	private Character character;

	private Effect_Management.Attribute_Manager attributes;

	// Use this for initialization
	void Start () {
		basicAttackCoolDown = 0;
		character = GetComponent<Character>();
		self = new Target(transform,true, false);
		attributes = new Effect_Management.Attribute_Manager();
	}

	void OnDestroy()
	{
		Debug.Log("destroying combat");
		self = null;
	}

	// Update is called once per frame
	void Update () {

		if(basicAttackCoolDown != 0 && System.DateTime.Now.Millisecond <= 20)
		{
			basicAttackCoolDown--;
		}

		if(target != null && target.location != null)
		{
			targetName = target.location.name;
		}else{
			targetName = "none";
		}

		attributes.stepTime();
		updateHealth();
		
	}

	/*---Utility Functions----------------------------------------------------------*/

	public bool beenDamaged()
	{
		return oldHealth > health;
	}

	public void updateHealth()
	{
		float healthChanges = (float) attributes.getHPChanges();

		//Debug.Log(healthChanges.ToString());
		health += healthChanges;

		oldHealth = health;
	}

	public float healthPercent()
	{
		return health / maxHealth;
	}
	
	float attackSpeed()
	{
		return baseAttackSpeed * 1; // * modifiers();
	}

	public bool targetWithin_AttackRange()
	{
		if (target.location != null && transform != null)
			return Vector3.Distance(target.location.position, transform.position) <= attackRange;
		else return false;
	}

	/*-------------------------------------------------------------*/


	/*---Combat Functions----------------------------------------------------------*/

	// Character takes physical damage
	public void recieve_Damage_Physical(float amount, ref Target enemy_target)
	{
		this.health -= amount;
		
		if (this.health <= 0)
		{	
			// deathAnimation();
			// createTimer_to_destory model();
			enemy_target.dead = true;
			enemy_target = null;
			die();
		}
	}

	// Character takes magic damage
	public void recieve_Damage_Magic(float amount,  Target enemy)
	{
		// magicResistance();
		this.health -= amount;
	}

	// Character is healed
	public void recieve_Healing(float amount)
	{
		// healingBuffs();
		this.health += amount;
	}

	// Character causes physical damage (Auto Attack)
	public void cause_Damage_Physical(Combat _target)
	{
		// damageBuffs();
		_target.recieve_Damage_Physical(damage, ref target);
	}

	public void autoAttack()
	{
		if(target != null && targetWithin_AttackRange()){

			transform.LookAt(target.location);

			if(basicAttackCoolDown <= 0)
			{
				if(isRanged)
				{
					GetComponentInChildren<Projectile_Launcher>().fire(target);
					basicAttackCoolDown = attackSpeed();
				}else{
					//Debug.Log("Attack!");
					cause_Damage_Physical(target.location.GetComponent<Combat>());
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
			self.dead = true;

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

	/*-------------------------------------------------------------*/

}
		                  
public class Target : IEquatable<Target> {
			
	public bool selectable;
	public bool dead;
	
	public Transform location;

	public bool Equals (Target right)
	{
		if (right == null) return false;

		return right.location.Equals(this.location) 
			&& right.selectable == this.selectable 
				&& right.dead == this.dead;
	}

	public Target(Transform loc, bool select, bool d)
	{
		location = loc;
		selectable = select;
		dead = d;
	}

	public override string ToString ()
	{
		return location.name;
	}
}
