using UnityEngine;
using System;
using System.Collections;


public enum attribute { HP, HPReg, MPReg, MaxMP, AR, AS, AP, AD, DO, ARM, MR, CDR};

namespace Effect_Management{

	public class Attribute_Manager {

        const int numAttributes = 12;

        Effect_Container<Attribute>[] attributes = new Effect_Container<Attribute>[numAttributes];

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

        public void removeHarmfulEffects()
        {
            foreach (var attribute in attributes)
            {
                attribute.removeHarmful();
            }
        }

	}

    public class Attribute_Effects
    {
        // Creates a new function that will ignore time and return a certain amount
        public static EffectApply  <Attribute> changeBy(double amount)
        {
            if (amount < 0) 
                 return ((x,y) => new Attribute(0.0, 0.0, amount, 0.0));
            else return ((x,y) => new Attribute(amount, 0.0, 0.0, 0.0));
        }

        // Creates a new function that will ignore time and return a certain amount
        public static EffectApply<Attribute> changeBy_percent(double amount)
        {
            if (amount < 0)
                return ((x,y) => new Attribute(0.0, 0.0, 0.0, amount));
            else return ((x,y) => new Attribute(0.0, amount, 0.0, 0.0));
        }

        // Creates a new function that will periodically change by a certain amount
        public static EffectApply<Attribute> periodic_changeBy(double period, double amount)
        {
            return Utility_Effects.periodic<Attribute>(changeBy(amount), period);
        }

    }

} // End of namespace
