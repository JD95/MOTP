using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

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
public class Effect_Container<T> where T : Affectable<T>{
	
	// A list of all last effects
	List<T> lasting_effects;
	
	// A list of time blocks for timed effects
	List<Time_Block<T>> timed_effects;
	
	// Go through each time block and get their effects for this time
	public T compileEffects()
	{
		T aggregate;
		
		// Goes through each time block and adds up the effects
		foreach(var block in timed_effects)
		{
			aggregate.add(block.getEffects());
		}
		
		return aggregate;
	}
	
	// For the lasting effects from sources like items or passives
	public void add_lastingEffect(Lasting_Effect<T> newEffect)
	{
		lasting_effects.Add(newEffect);
	}
	
	// For all timed effects
	public void add_timedEffect(Timed_Effect<T> newEffect)
	{
		// Find the appropriate timeblock that matches the stop 
		Time_Block<T> match = tryToMatchTime(newEffect.getStopTime());
		
		if (match == null)
		{
			List<Timed_Effect<T>> newList = new List<Timed_Effect<T>>().Add(newEffect);
			
			// Create a new Time block and add it to the list of effects
			timed_effects.Add(new Time_Block<T>(newEffect.getStopTime(), newList));
			
			// Sort the list so the front is still the next to be removed
			timed_effects.Sort((a,b) => a.getStopTime().CompareTo(b.getStopTime()));
			
		}else{
			match.addEffect(newEffect);
		}
		
	}
	
	Time_Block<T> tryToMatchTime(DateTime time)
	{
		timed_effects.Find(x => x.getStopTime().Equals(time));
	}
	
	public void stepTime()
	{
		if(timed_effects.First().expiredCheck())
		{
			// Tell all effects in the first block to stop
			timed_effects.First().stop_effects();
			
			// Remove the first block
			timed_effects.RemoveAt(0);
		}
	}
}
