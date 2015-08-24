using UnityEngine;
using System.Collections;
using System;

namespace Effect_Management{

	public delegate T EffectApply<T>(DateTime time, int stacks) where T : Affectable<T>, new();
	public delegate void EffectStop();

    public enum EffectType { Bleed, Posion, Slow, Stun };

    public class effectInfo
    {
        private string name;
        private EffectType type;
        private int stacks = 0;
        private double duration = 0;
        private DateTime stopTime; // When the effect should stop

        public effectInfo(string _name, EffectType _type, int _stacks, double _duration_seconds, DateTime birthTime)
        {
            name     = _name;
            type     = _type;
            stacks   = _stacks;
            duration = _duration_seconds;
            stopTime = birthTime.Add(TimeSpan.FromSeconds(_duration_seconds));
            
        }

        public DateTime getStopTime()
        {
            return stopTime;
        }

        public int getStacks()
        {
            return stacks;
        }

        public void addStack()
        {
            stacks++;
            stopTime = DateTime.Now.Add(TimeSpan.FromSeconds(duration));
        }

        public string getName()
        {
            return name;
        }

        public EffectType getEffectType()
        {
            return type;
        }

    }

	public class Timed_Effect<T> {

        public effectInfo info;

		private EffectApply<T> app;

		private EffectStop end;

		// Provide both the time the effect begins and the duration
		// What happens at certain times and what happens when the event ends?
		public Timed_Effect (effectInfo _info, EffectApply<T> _app, EffectStop _stop)
		{
            info = _info;
			app = _app;
			end = _stop;
		}

		// Depending on the time and number of stacks, return either an effect or a "zero" value
		public T apply(DateTime time)
		{
			T test = app(time, info.getStacks());
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
