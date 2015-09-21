using UnityEngine;
using System;
using System.Collections.Generic;



public enum attribute { HP, HPReg, MPReg, MaxMP, AR, AS, AP, AD, DO, ARM, MR, CDR};

namespace Effect_Management{

	public class Attribute_Manager {

        /*
         *  These tables serve to fix the problem of casting spells over the network. Because we cannot
         *  send anything more complex than ints or strings through the server, we cannot directly apply
         *  effects. Instead we want to pass a string (the name of the effect) to one of these tables in
         *  the effect manager, and then the name would be looked up in the table and then return to new
         *  effect to apply.
         *  
         *  Because abilities are classes, we can add the effect (if any) to our table when the object is
         *  constructed or in Start(), which ever is more suitable. The key for the hash table is going to
         *  be the name of the effect.
         * 
         */ 
        public static TimedEffect_table<Attribute> timedEffects = new TimedEffect_table<Attribute>();
        public static LastingEffect_table<Attribute> lastingEffects = new LastingEffect_table<Attribute>();

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

        public void addTimedEffectFor(attribute attr, string effect)
        {
            attributes[(int)attr].add_timedEffect(timedEffects[effect]());
        }

        public void addLastingEffectFor(attribute attr, string effect)
        {
            attributes[(int)attr].add_lastingEffect(lastingEffects[effect]());
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
