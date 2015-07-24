using UnityEngine;
using System;
using System.Collections;

namespace Effect_Management {

	public class CharacterState_Manager {

		Effect_Container<CharacterState> living;
		public void applyLivingChanges(GameObject character)
		{
		  living.compileEffects().value(character);
		}
		public void addLivingChange(GameObject character)
		{
			living.add_timedEffect(new Timed_Effect<CharacterState>(
				DateTime.Now,								// Birth time
				10.0,										// Durration in seconds
				x => CharacterState.zero(),	// Application funtion
				CharacterState_Effects.destroyCharacter(character)
				));	// Stopping function
		}
		
		public CharacterState_Manager(GameObject character)
		{
			living = new Effect_Container<CharacterState>();

		}
		
		public void stepTime()
		{
			living.stepTime();
		}


		private class CharacterState_Effects {
		// Transforms a constant EffectApply function into a periodic one
			private static EffectApply<CharacterState> periodic (EffectApply<CharacterState> app, double period_secs)
			{
				DateTime startTime = DateTime.Now;
				
				return ( x => x.Subtract(startTime).Duration().TotalSeconds % period_secs < 0.01 ? 
				        app(x) : CharacterState.zero());
			}

			public static void doNothing(){}

			public static EffectStop destroyCharacter(GameObject character)
			{
				return () => {
					GameObject.Destroy(character);
					//character.SetActive(false);
					//GameObject.Destroy(character);
				};
			}
		}
	}
}