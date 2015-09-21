using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Effect_Management{
/*
 * Effect containers manage all of the effects pertaining to
 * a certain target. (eg. movement speed effects). The container
 * is consulted whenever that target needs to be used so all of
 * the effects can be taken into account. Doing this prevents
 * errors in the effects from permanently chaning the attributes
 * they are bound to.
 * 
 * 
 */ 
	public class Effect_Container<T> where T : Affectable<T>, new(){
		
		// A list of all last effects
		List<Lasting_Effect<T>> lasting_effects;
		
		// A list of time blocks for timed effects
		List<Time_Block<T>> timed_effects;

		// Constructor
		public Effect_Container()
		{
			lasting_effects = new List<Lasting_Effect<T>>();
			timed_effects 	= new List<Time_Block<T>>();
		}

		// Go through each time block and get their effects for this time
		public T compileEffects()
		{
			T aggregate; aggregate = new T();
            DateTime now = DateTime.Now;

            // Adds up all lasting effects
            foreach (var effect in lasting_effects)
            {
               aggregate = aggregate.add(effect.apply(now));
            }

			// Goes through each time block and adds up the effects
			foreach(var block in timed_effects)
			{
				T effectResult = block.getEffects(now);
				aggregate = aggregate.add(effectResult);
			}

			return aggregate;
		}
		
		// For the lasting effects from sources like items or passives
		public void add_lastingEffect(Lasting_Effect<T> newEffect)
		{
			lasting_effects.Add(newEffect);
		}

        public void remove_lastingEffect(string id)
        {
            lasting_effects = lasting_effects.Where(x => x.id != id).ToList();
        }
		
		// For all timed effects
		public void add_timedEffect(Timed_Effect<T> newEffect)
		{
            // If the ability is already being applied, add a stack
            if (tryToAddStack(newEffect.info.getName())) return;

			// Find the appropriate timeblock that matches the stop 
			Time_Block<T> match = tryToMatchTime(newEffect.info.getStopTime());
			
			if (match == null)
			{
				// Create new effect list
				List<Timed_Effect<T>> newList = new List<Timed_Effect<T>>();
				
				// Add the effect to the new List
				newList.Add(newEffect);
				
				// Create a new Time block and add it to the list of timed effects
				timed_effects.Add(new Time_Block<T>(newEffect.info.getStopTime(), newList));
				
			}else{
				
				// Add effect to existing list
				match.addEffect(newEffect);
			}
			
		}

        bool tryToAddStack(string effectName)
        {
            foreach (var timedBlock in timed_effects)
            {
                var potential = timedBlock.has(effectName);
                if(potential != null)
                {
                    potential.info.addStack();
                    return true;
                }
            }

            return false;
        }
		
		Time_Block<T> tryToMatchTime(DateTime time)
		{
			return timed_effects.Find(x => x.getStopTime().Equals(time));
		}

        public void removeHarmful()
        {
            foreach (var timeBlock in timed_effects)
            {
                timeBlock.removeHarmful();
            }
        }

		// Tick through time for the container
		public void stepTime()
		{

			// If there are timed elements and the first one is expired
			if(timed_effects.Count != 0 && timed_effects.First().expiredCheck())
			{
				// Tell all effects in the first block to stop
				timed_effects.First().stop_effects();
				
				// Remove the first block
				timed_effects.RemoveAt(0);
			}
		}

	}

    /*
     * These "classes" are just so we dont have to write Dictionary<Func<Timed_Effect<T>>, string> in places
     * Essentially they are a table of fucntions that return new effects. The reason for returning functions
     * is that, in the case of timed effects, the actual time the object is created is important, so the 
     * effects must be created on the fly!
     */
    public class TimedEffect_table<T> : Dictionary<string, Func<Timed_Effect<T>>> { }
    public class LastingEffect_table<T> : Dictionary<string, Func<Lasting_Effect<T>>> { }

} // End of namespace
