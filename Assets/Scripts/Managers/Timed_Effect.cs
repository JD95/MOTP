using UnityEngine;
using System.Collections;
using System;

namespace Effect_Management{

	public delegate T EffectApply<T>(DateTime time) where T : Affectable<T>, new();
	public delegate void EffectStop();

	public class Timed_Effect<T> {

		// When the effect should stop
		private DateTime stopTime;

		private EffectApply<T> app;

		private EffectStop end;
		
		public DateTime getStopTime()
		{
			return stopTime;
		}

		// Provide both the time the effect begins and the duration
		// What happens at certain times and what happens when the event ends?
		public Timed_Effect (DateTime birthTime, double duration_seconds, EffectApply<T> _app, EffectStop _stop)
		{
			stopTime = birthTime.Add(TimeSpan.FromSeconds(duration_seconds));
			app = _app;
			end = _stop;
		}

		// Depending on the time, return either an effect or a "zero" value
		public T apply(DateTime time)
		{
			T test = app(time);
			//Debug.Log("app returned " + test.ToString());
			return test;

		}

		// What is done when the effect stops
		public void stop()
		{
			end();
		}
	}
}
