using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Effect_Management {

/*
 * Time blocks group effects that have a similar stop time.
 * By grouping effects together and sorting the timeblocks
 * we can quickly remove effects when they stop and we only
 * have to check the frontmost block for expiration!
 * 
 */ 
	public class Time_Block<T> where T : Affectable<T>, new()
	{
		DateTime stopTime;

		List<Timed_Effect<T>> effects;

		public Time_Block(DateTime _stopTime, List<Timed_Effect<T>> _effects)
		{
			stopTime = _stopTime;
			effects  = _effects;
		}

		public DateTime getStopTime()
		{
			return stopTime;
		}

		// Check whether current time block has expired
		public bool expiredCheck()
		{
			switch (DateTime.Now.CompareTo(stopTime))
			{
				// stop time is before current time
			case -1: return false;
				
				// it is currently stop time
			case  0: return true;
				
				// it is not yet stop time
			case  1: return true;
				
				//default case
			default: return false;
			}
		}

		// Stop all effects in this time block
		public void stop_effects()
		{
			foreach (Timed_Effect<T> effect in effects)
			{
				effect.stop();
			}
		}

		public T getEffects(DateTime time)
		{
			T aggregate; aggregate = new T();
			
			// Gathers up all the effects for this time
			foreach(Timed_Effect<T> effect in effects)
			{
				T effectResult = effect.apply(time);

				// What ever the T type is, it must be able to combine the effects
				aggregate = aggregate.add(effectResult);
			}

			return aggregate;
		}

		public void addEffect(Timed_Effect<T> newEffect)
		{
			effects.Add(newEffect);
		}

        public void removeHarmful()
        {
            effects = effects.Where(x => 
            {
                var type = x.info.getEffectType();
                return !(type == EffectType.Bleed ||
                         type == EffectType.Posion ||
                         type == EffectType.Slow ||
                         type == EffectType.Stun);
            }).ToList();
        }

        public Timed_Effect<T> has(string effectName)
        {
            return effects.Where(x => x.info.getName() == effectName).First();
        }
	}
}