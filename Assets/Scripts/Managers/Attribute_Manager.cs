using UnityEngine;
using System;
using System.Collections;

namespace Effect_Management{

	public class Attribute_Manager {

		Effect_Container<Attribute> HP;
		public double getHPChanges()
		{
			return HP.compileEffects().value;
		}

		public Attribute_Manager()
		{
			HP = new Effect_Container<Attribute>();
			/* HP.add_timedEffect(new Timed_Effect<Attribute>(
				DateTime.Now,								// Birth time
				10.0,										// Durration in seconds
				Attribute_Effects.periodic_changeBy(5.0,-1.0),	// Application funtion
				Attribute_Effects.doNothing));							// Stopping function
				*/
		}

		public void stepTime()
		{
			HP.stepTime();
		}

		private class Attribute_Effects {
			
			// Transforms a constant EffectApply function into a periodic one
			private static EffectApply<Attribute> periodic (EffectApply<Attribute> app, double period_secs)
			{
				DateTime startTime = DateTime.Now;
				
				return ( x => x.Subtract(startTime).Duration().TotalSeconds % period_secs < 0.01 ? 
				        app(x) : Attribute.zero());
			}
			
			// Creates a new function that will ignore time and return a certain amount
			public static EffectApply<Attribute> changeBy (double amount)
			{
				return (x => new Attribute(amount));
			}
			
			// Creates a new function that will periodically change by a certain amount
			public static EffectApply<Attribute> periodic_changeBy (double period, double amount)
			{
				return periodic(changeBy(amount), period);
			}
			
			public static void doNothing() {}
		}
	}


} // End of namespace
