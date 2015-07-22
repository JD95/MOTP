using UnityEngine;
using System.Collections;

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
public interface Affectable<T> {
	
	T add(T other);
	T zero();
}

public class Attribute : Affectable<Attribute>{

	public double value;

	public Attribute (double _value)
	{
		value = _value;
	}

	Attribute add(Attribute other)
	{
		return new Attribute(other.value + this.value);
	}

	Attribute zero()
	{
		return new Attribute(0);
	}
}