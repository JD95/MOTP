using UnityEngine;
using System;
using System.Collections;

namespace Effect_Management{

	public class Attribute_Manager {

        public enum attribute {HP,HPReg,AR,AS,AP,AD};

		Effect_Container<Attribute> HP; // Health Points
        public Attribute getHP_Changes() { return HP.compileEffects(); }

        Effect_Container<Attribute> HPReg; // Health Regen
        public Attribute getHPReg_Changes() { return HPReg.compileEffects(); }

        Effect_Container<Attribute> AR; // Attack Range
        public Attribute getAR_Changes() { return AR.compileEffects(); }

        Effect_Container<Attribute> AS; // Attack Speed
        public Attribute getAS_Changes() { return AS.compileEffects(); }

        Effect_Container<Attribute> AP; // Attack Power
        public Attribute getAP_Changes() { return AP.compileEffects(); }

        Effect_Container<Attribute> AD; // Attack Damage
        public Attribute getAD_Changes() { return AD.compileEffects(); }


		public Attribute_Manager()
		{
			HP      = new Effect_Container<Attribute>();
            HPReg   = new Effect_Container<Attribute>();
            AR      = new Effect_Container<Attribute>();
            AS      = new Effect_Container<Attribute>();
            AP      = new Effect_Container<Attribute>();
            AD      = new Effect_Container<Attribute>();
		}

		public void stepTime()
		{
			HP.stepTime();
            HPReg.stepTime();
            AR.stepTime();
            AS.stepTime();
            AP.stepTime();
            AD.stepTime();
		}

        // This is inside the Attribute_Manager class because we cannot decalre private classes in a namepsace
        private class Attribute_Effects
        {

            // Transforms a constant EffectApply function into a periodic one
            private static EffectApply<Attribute> periodic(EffectApply<Attribute> app, double period_secs)
            {
                DateTime startTime = DateTime.Now;

                return (x => x.Subtract(startTime).Duration().TotalSeconds % period_secs < 0.01 ?
                        app(x) : Attribute.zero());
            }

            // Creates a new function that will ignore time and return a certain amount
            public static EffectApply<Attribute> changeBy(double amount)
            {
                return (x => new Attribute(amount,1));
            }

            // Creates a new function that will periodically change by a certain amount
            public static EffectApply<Attribute> periodic_changeBy(double period, double amount)
            {
                return periodic(changeBy(amount), period);
            }

            public static void doNothing() { }
        }
	}


} // End of namespace
