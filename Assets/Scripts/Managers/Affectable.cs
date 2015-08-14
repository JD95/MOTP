using UnityEngine;
using System.Collections;

namespace Effect_Management{
/*
 * 	Affectable T defines a range of types that
 * 	can be managed by effect mangers. All of these
 * 	types need to define an add operation as well
 * 	as define a zero unit within that type.
 * 
 * 	The following rule should be followed:
 * 		
 * 		T a; T other;
 * 
 * 		a == a.add(zero);
 * 		a != a.add(other);
 * 
 * 	It is up to you how to define add, whether it be
 * 	simplying adding numbers contained in T like you
 * 	would with attributes for champs, or combining graphical
 * 	changes as you would with graphical effects.
 */

    using directValue       = System.Double;
    using percentValue      = System.Double;
    using AttributeChange   = Tuple<directValue, percentValue>;

	public abstract class Affectable<T> where T : new(){

		public static void doNothing(){} // Some zero functions need a doNothing value

		public abstract T add(T other);

		public static T zero()
		{
			return new T();
		}
	}

	// The value that attribute effects produce
	public class Attribute : Affectable<Attribute>{
        
		public Tuple<directValue,percentValue> value;

		public Attribute () {
            value = new Tuple<directValue, percentValue>(0.0, 0.0);
		}

		public Attribute (double _direct, double _percent)
		{
            value = new Tuple<directValue, percentValue>(_direct, _percent);
		}

		public override Attribute add(Attribute other)
		{
			Attribute test = new Attribute(other.value.First  + this.value.First, 
                                           other.value.Second + this.value.Second);
			return test;
		}

        public double applyTo(double initial)
        {
            return (initial + value.First) * (1.0 + value.Second);
        }

		public override string ToString ()
		{
			return value.ToString();
		}
	}

	/*
	 * Graphical effects are collected by their container and then
	 * executed all together. Thus the value of the Graphical is a
	 * function.
	 * 
	 */
	public class Graphical : Affectable<Graphical>
	{
		public delegate void Update();

		public Update value;

		public Graphical()
		{
			value = doNothing;
		}

		public Graphical(Update other)
		{
			value = other;
		}

		public override Graphical add(Graphical other)
		{
			return new Graphical(()=>
			        {
						value();
						other.value();
					});// Do this value, then do the next one
		}
	}

	// Like graphical effects, CharacterState effects return functions of those changes
	// Requires a game object to work with though
	public class CharacterState : Affectable<CharacterState>
	{
		public delegate void Update(GameObject obj);

		public Update value;

		public CharacterState()
		{
			value = (x => doNothing());
		}

		public CharacterState(Update other)
		{
			value = other;
		}

		public override CharacterState add (CharacterState other)
		{
			return new CharacterState(x => {value(x); other.value(x);});
		}
	}

} //End of namespace