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
	public abstract class Affectable<T> where T : new(){
		
		public abstract T add(T other);
		public static T zero()
		{
			return new T();
		}
	}

public class Attribute : Affectable<Attribute>{

		public double value;

		public Attribute () {
			value = 0;
		}

		public Attribute (double _value)
		{
			value = _value;
		}

		public override Attribute add(Attribute other)
		{
			Attribute test = new Attribute(other.value + this.value);
			//Debug.Log("I ran add! I got " + test.ToString());
			return test;
		}

		public override string ToString ()
		{
			return value.ToString();
		}
	}
}