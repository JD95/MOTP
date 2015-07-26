using UnityEngine;
using System.Collections;


public enum Animations {idle, run, attack, die, gethit};

public class Character : MonoBehaviour
{
	static int numchars;
	private Texture2D backgroundTexture;
	public Texture2D healthTexture;

	public string team;

	// Map stuff
	public int charID = 0;
	public bool isHero = false;
	public bool isBase = false;

	// Character Model
	public GUISkin skin;
	public Transform bloodPrefab;
	public Animation avatar;

	private float speed;
	private NavMeshAgent navigation;

	//private CharacterController movementControler;
	private Combat combatData;

	public Animations currentAnimation = Animations.idle;

	public Effect_Management.CharacterState_Manager characterState;	

	void Awake ()
	{


		//movementControler = GetComponent<CharacterController>();
		combatData = GetComponent<Combat>();
		navigation = GetComponent<NavMeshAgent>();

		backgroundTexture = new Texture2D (1, 1, TextureFormat.RGB24, false);
		backgroundTexture.SetPixel (0, 0, Color.black);
		backgroundTexture.Apply ();

		if(!isBase){
			charID = numchars++;
			gameObject.name = "char" + charID;
		}
		speed = 10.0F;
	}

	void Start()
	{
		characterState = new Effect_Management.CharacterState_Manager(gameObject);
	}

	void Update ()
	{
		// Damage has been done to character, so make blood
		if (combatData.beenDamaged()) {
			Instantiate (bloodPrefab, transform.position, transform.rotation);
			GetComponent<AudioSource>().Play();
		}

		// Perform next animation
		animate ();

		// We are done with the health so it swaps old with new
		combatData.updateHealth();

		characterState.stepTime();
		//characterState.applyLivingChanges(game);
	}


	void animate()
	{
		if(avatar == null) return;

		switch (currentAnimation)
		{
		case Animations.idle:
			avatar.CrossFade("idle",0.5f); break;
		case Animations.run:
			avatar.CrossFade("run",0.1f); break;
		case Animations.attack:
			avatar.CrossFade("attack",0.1f); break;
		case Animations.gethit:
			avatar.CrossFade("gethit",0.1f); break;
		}
	}

	public bool within_Destination()
	{
		return Vector3.Distance(navigation.destination, transform.position).AlmostEquals(navigation.stoppingDistance,1f);
	}

	public void moveTo (Transform location)
	{
		if (location == null) return;



		// If its asking to move to your current destination
		if (navigation.destination == location.position)
		{
			// If you are close enough to destination
			if(within_Destination())
			{
				currentAnimation = Animations.idle;
			}else{
				currentAnimation = Animations.run;
			}
		}else {
			// I have new destination
			navigation.SetDestination(location.position);
			navigation.Resume();
			//moveTo(location);
		}

		//movementControler.SimpleMove(direction * speed);
	}

	public void stopNavigation()
	{
		navigation.Stop();
	}

	// Health Bar (all) and level (hero only)
	void OnGUI ()
	{
		if (GameManager.paused)
			return;

		if(combatData.health <= 0)
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
			
		float healthPercent = combatData.healthPercent();
		GUI.DrawTexture (new Rect (2, 2, innerBarWidth * healthPercent, innerBarHeight), healthTexture, ScaleMode.StretchToFill);
		
		GUI.EndGroup ();
		
		if (isHero) {
			GUI.Label(new Rect (viewPos.x - 50, posYHealthBar - 23, 100, 25), "Level "+combatData.level);
		}
			
	}
	
//	public Vector3 getVelocity()
//	{
//		return movementControler.velocity;
//	}

	public Vector3 getPosition()
	{
		return transform.position;
	}
}
