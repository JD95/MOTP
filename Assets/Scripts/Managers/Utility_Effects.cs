using UnityEngine;
using System;
using System.Collections;


namespace Effect_Management
{
    public class Utility_Effects
    {

        // Transforms a constant EffectApply function into a periodic one
        public static EffectApply<T> periodic<T>(EffectApply<T> app, double period_secs) where T : new()
        {
            DateTime startTime = DateTime.Now;

            return ((x,y) => x.Subtract(startTime).Duration().TotalSeconds % period_secs < 0.01 ?
                    app(x,y) : Affectable<T>.zero());
        }

        public static EffectStop doNothing_Stop() { return (() => {}); }

    }
}
