using UnityEngine;
using System.Collections;

/*
 * 	Handles all combat functionality. Location of Health and other combat data
 * 	like attack range and speed.
 * 
 */

public class Combat : MonoBehaviour {

	public float health;
	private float oldHealth;
	
	public float maxHealth;
	
	public int creeps = 0;
	public int level = 1;
	
	public float range = 2;
	public float damage = 1.5f;

	public Transform target;

	// Combat
	private float lastAttackTime;

	public float attackRange;
	public float baseAttackSpeed;
	
	private Character character;

	// Use this for initialization
	void Start () {
		lastAttackTime = 0;
		character = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*---Utility Functions----------------------------------------------------------*/

	public bool beenDamaged()
	{
		return oldHealth > health;
	}

	public void updateHealth()
	{
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
		if (target != null)
			return Vector3.Distance(target.position, transform.position) <= attackRange;
		else return false;
	}

	/*-------------------------------------------------------------*/


	/*---Combat Functions----------------------------------------------------------*/

	// Character takes physical damage
	public void recieve_Damage_Physical(float amount)
	{
		this.health -= amount;
		
		if (this.health <= 0)
		{	
			// deathAnimation();
			// createTimer_to_destory model();
			GameObject.Destroy(gameObject);
		}
	}

	// Character takes magic damage
	public void recieve_Damage_Magic(float amount)
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
	public void cause_Damage_Physical(Combat target)
	{
		// damageBuffs();
		target.recieve_Damage_Physical(damage);
	}

	public void autoAttack()
	{
		transform.LookAt(target);

		if(target != null && Time.time - lastAttackTime > attackSpeed() &&
		   targetWithin_AttackRange() )
		{
			//Debug.Log("Attack!");
			cause_Damage_Physical(target.GetComponent<Combat>());
			lastAttackTime = Time.time;
		}

		character.currentAnimation = Animations.attack;
	}

	/*-------------------------------------------------------------*/

}
