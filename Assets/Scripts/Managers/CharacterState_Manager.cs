using UnityEngine;
using System;
using System.Collections;

namespace Effect_Management {

	public class CharacterState_Manager {

		Effect_Container<CharacterState> state;
		public void applyCharacterChanges(GameObject character)
		{
            state.compileEffects().value(character);
		}

		public void addTimedEffect(Timed_Effect<CharacterState> effect)
		{
            state.add_timedEffect(effect);
		}
		
		public CharacterState_Manager(GameObject character)
		{
            state = new Effect_Container<CharacterState>();
		}
		
		public void stepTime()
		{
            state.stepTime();
		}

	}

    public class CharacterState_Effects
    {
        // Transforms a constant EffectApply function into a periodic one

        public static EffectApply<CharacterState> doNothing()
        {
            return (x, y) => CharacterState.zero();
        }

        private static EffectStop destroyCharacter(GameObject character)
        {
            return () => { GameObject.Destroy(character); };
        }

        private static EffectStop respawnCharacter(GameObject character)
        {
            return () =>
            {
                var manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                var combatData = character.GetComponent<Combat>();
                var characterData = character.GetComponent<Character>();

                // Consider character to be alive
                combatData.dead = false;
                combatData.selectable = true;

                // Restore health
                combatData.health = combatData.maxHealth;
                combatData.mana = combatData.maxMana();

                // Move Character to correct position
                var spawn = character.tag == "TeamA" ?
                        manager.bluespawn[UnityEngine.Random.Range(0, manager.bluespawn.Length)] :
                        manager.redspawn[UnityEngine.Random.Range(0, manager.redspawn.Length)];
                character.transform.position = spawn.transform.position;

                // Set animation state to alive
                characterData.setAnimation_State(characterData.dead_State, false);

                // Enable movement
                character.GetComponent<NavMeshAgent>().enabled = true;
                character.GetComponent<Rigidbody>().useGravity = true;
            };
        }

        public static Timed_Effect<CharacterState> destroyCharacterObject(GameObject character)
        {
            return new Timed_Effect<CharacterState>(
                   new effectInfo("Death", EffectType.Posion, 1, 10.0, DateTime.Now),
                   doNothing(),
                   destroyCharacter(character));
        }

        public static Timed_Effect<CharacterState> respawnHero(GameObject character)
        {
            return new Timed_Effect<CharacterState>(
                new effectInfo("Respawn", EffectType.Posion, 1, 2.0, DateTime.Now),
                doNothing(),
                respawnCharacter(character));
        }
    }
}