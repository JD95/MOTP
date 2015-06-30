using UnityEngine;
using System.Collections;


public enum Animations {idle, walking};

public class Character : MonoBehaviour
{
	static int numchars;
	private Texture2D backgroundTexture;
	public Texture2D healthTexture;

	public string team;

	// Combat info
	public float health;
	private float oldHealth;

	public float maxHealth;

	public int creeps = 0;
	public int level = 1;

	public float range = 2;
	public float damage = 1.5f;

	// Map stuff
	public int charID = 0;
	public bool isHero = false;
	public bool isBase = false;

	public float xp = 0;

	// Character Model
	public GUISkin skin;
	public Transform bloodPrefab;
	public Animation avatar;

	private float speed;

	private CharacterController movementControler;

	public Animations currentAnimation = Animations.idle;

	void Awake ()
	{
		movementControler = GetComponent<CharacterController>();

		backgroundTexture = new Texture2D (1, 1, TextureFormat.RGB24, false);
		backgroundTexture.SetPixel (0, 0, Color.black);
		backgroundTexture.Apply ();

		charID = numchars++;
		gameObject.name = "char" + charID;

		speed = 10.0F;
	}

	void Update ()
	{
		// Damage has been done to character
		if (oldHealth > health) {
			Instantiate (bloodPrefab, transform.position, transform.rotation);
			GetComponent<AudioSource>().Play();
		}

		animate ();

		// Set the health
		oldHealth = health;
	}


	void animate()
	{
		switch (currentAnimation)
		{
		case Animations.idle:
			avatar.CrossFade("idle",0.5f); break;
		case Animations.walking:
			avatar.CrossFade("walk-cicle",0.1f); break;
		}
	}

	public void moveTo (Waypoint location)
	{
		Vector3 heading = location.getPosition() - transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		movementControler.SimpleMove(direction * speed);
	}

	public void Xp(float xp) {
		this.xp += xp;
		if (level < 10 && this.xp >= 5) {
			level++;
			this.health = maxHealth;
			this.xp = 0;
			if (level >= 10)
				level = 10;
		}
	}

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

	public void recieve_Damage_Magic(float amount)
	{
		// magicResistance();
		this.health -= amount;
	}

	public void recieve_Healing(float amount)
	{
		// healingBuffs();
		this.health += amount;
	}

	public void cause_Damage_Physical(Character target)
	{
		// damageBuffs();
		target.recieve_Damage_Physical(10.0f);
	}
		
	// Health Bar (all) and level (hero only)
	void OnGUI ()
	{
		if (GameManager.paused)
			return;

		GUI.skin = skin;
		GUI.depth = 3;

		Vector2 backgroundBarSize = new Vector2 (Screen.width * 0.2f, Screen.height * 0.06f);
			
		Vector3 viewPos = Camera.main.WorldToScreenPoint (this.transform.position + new Vector3 (0, 3, 0));
			
		float valueZ = viewPos.z;
		if (valueZ < 1) {
			valueZ = 1;
		} else if (valueZ > 4) {
			valueZ = 4;
		}
		float valueToNormalize = Mathf.Abs (1 / (valueZ - 0.5f));
			
		int backgroundBarWidth = (int)(backgroundBarSize.x * valueToNormalize);
		if (backgroundBarWidth % 2 != 0) {
			backgroundBarWidth++;
		}
		float backgroundBarHeight = (int)(backgroundBarSize.y * valueToNormalize);
		if (backgroundBarHeight % 2 != 0) {
			backgroundBarHeight++;
		}
			
		float innerBarWidth = backgroundBarWidth - 2 * 2;
		float innerBarHeight = backgroundBarHeight - 2 * 2;
			
			
		float posYHealthBar = Screen.height - viewPos.y - backgroundBarHeight;
			
		GUI.BeginGroup (new Rect (viewPos.x - backgroundBarWidth / 2, posYHealthBar, backgroundBarWidth, backgroundBarHeight));
		GUI.DrawTexture (new Rect (0, 0, backgroundBarWidth, backgroundBarHeight), backgroundTexture, ScaleMode.StretchToFill);
			
		float healthPercent = (health / maxHealth);
		GUI.DrawTexture (new Rect (2, 2, innerBarWidth * healthPercent, innerBarHeight), healthTexture, ScaleMode.StretchToFill);
		
		GUI.EndGroup ();
		
		if (isHero) {
			GUI.Label(new Rect (viewPos.x - 50, posYHealthBar - 23, 100, 25), "Level "+level);
		}
			
	}
	
	public Vector3 getVelocity()
	{
		return movementControler.velocity;
	}

	public Vector3 getPosition()
	{
		return transform.position;
	}
}
