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
			// Find the appropriate timeblock that matches the stop 
			Time_Block<T> match = tryToMatchTime(newEffect.getStopTime());
			
			if (match == null)
			{
				// Create new effect list
				List<Timed_Effect<T>> newList = new List<Timed_Effect<T>>();
				
				// Add the effect to the new List
				newList.Add(newEffect);
				
				// Create a new Time block and add it to the list of timed effects
				timed_effects.Add(new Time_Block<T>(newEffect.getStopTime(), newList));
				
				// Sort the list so the front is still the next to be removed
				//timed_effects.Sort((a,b) => a.getStopTime().CompareTo(b.getStopTime()));
				
			}else{
				
				// Add effect to existing list
				match.addEffect(newEffect);
			}
			
		}
		
		Time_Block<T> tryToMatchTime(DateTime time)
		{
			return timed_effects.Find(x => x.getStopTime().Equals(time));
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



} // End of namespace
