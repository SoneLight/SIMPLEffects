using System;
using System.Text;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes

namespace SIMPLEffects
{
    public class RampMinusFX : Effect
    {
        protected override int getPhaseValue(float phase)
        {
            if (phase < width)
                return maxValue - (int)((maxValue - minValue) * (phase / width));
            else
                return minValue;
        }
    }
}
