using UnityEngine;
using System.Collections;


public enum Animations {idle, run, attack, die, gethit};

public class Character : MonoBehaviour
{
	static int numchars;

	// Map stuff
	public int charID = 0;
	public bool isHero = false;
	public bool isBase = false;

	// Character Model
    private Animator anim;
    public int running_State = Animator.StringToHash("Running");
    public int attacking_State = Animator.StringToHash("Attacking");
    public int dead_State = Animator.StringToHash("Dead");


    private Combat combatData;

	public Transform bloodPrefab;
	
	public Animations currentAnimation = Animations.idle;

	public Effect_Management.CharacterState_Manager characterState;	

	void Awake ()
	{
		if(!isBase && !isHero){
			charID = numchars++;
			gameObject.name = "char" + charID;
		}
	}

	void Start()
	{
        anim = GetComponent<Animator>();
        combatData = GetComponent<Combat>();
		characterState = new Effect_Management.CharacterState_Manager(gameObject);
	}

	void Update ()
	{
		// Damage has been done to character, so make blood
		if (combatData.beenDamaged()) {
			Instantiate (bloodPrefab, transform.position, transform.rotation);
			GetComponent<AudioSource>().Play();
		}

		characterState.stepTime();
	}

    public void setAnimation_State(int id, bool state)
    {
        anim.SetBool(id, state);
    }

	public Vector3 getPosition()
	{
		return transform.position;
	}
}
