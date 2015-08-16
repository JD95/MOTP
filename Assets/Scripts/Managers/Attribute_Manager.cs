﻿using UnityEngine;
using System;
using System.Collections;


public enum attribute { HP = 0, HPReg = 1, AR = 2, AS = 3, AP = 4, AD = 5, DO = 6 };

namespace Effect_Management{

	public class Attribute_Manager {

        Effect_Container<Attribute>[] attributes = new Effect_Container<Attribute>[7];

        public Attribute_Manager()
        {
            for(int i = 0; i < attributes.Length; i++)
            {
                attributes[i] = new Effect_Container<Attribute>();
            }
        }

		public void stepTime()
		{
            foreach(var attr in attributes)
            {
                attr.stepTime();
            }

		}

        public Attribute getChangesFor(attribute attr)
        {
            return attributes[(int)attr].compileEffects();
        }

        public void addTimedEffectFor(attribute attr, Timed_Effect<Attribute> newEffect)
        {
            attributes[(int)attr].add_timedEffect(newEffect);
        }

        public void addLastingEffectFor(attribute attr, Lasting_Effect<Attribute> newEffect)
        {
            attributes[(int)attr].add_lastingEffect(newEffect);
        }

        public void removeLastingEffectFor(attribute attr, string id)
        {
            attributes[(int)attr].remove_lastingEffect(id);
        }

	}

    public class Attribute_Effects
    {
        // Creates a new function that will ignore time and return a certain amount
        public static EffectApply  <Attribute> changeBy(double amount)
        {
            if (amount < 0) 
                 return (x => new Attribute(0.0, 0.0, amount, 0.0));
            else return (x => new Attribute(amount, 0.0, 0.0, 0.0));
        }

        // Creates a new function that will ignore time and return a certain amount
        public static EffectApply<Attribute> changeBy_percent(double amount)
        {
            if (amount < 0)
                return (x => new Attribute(0.0, 0.0, 0.0, amount));
            else return (x => new Attribute(0.0, amount, 0.0, 0.0));
        }

        // Creates a new function that will periodically change by a certain amount
        public static EffectApply<Attribute> periodic_changeBy(double period, double amount)
        {
            return Utility_Effects.periodic<Attribute>(changeBy(amount), period);
        }

    }

} // End of namespace
