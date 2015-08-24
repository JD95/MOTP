using UnityEngine;
using System;
using System.Collections;

namespace Effect_Management{

	public class Lasting_Effect<T> where T : Affectable<T>, new()
    {
        public string id;
        public int stacks = 0;

        private EffectApply<T> app;

        public Lasting_Effect (string _id, EffectApply<T> _app)
        {
            this.id = _id;
            this.app = _app;
        }

        // Depending on the time, return either an effect or a "zero" value
        public T apply(DateTime time)
        {
            T test = app(time, stacks);
            return test;

        }

	}
}
