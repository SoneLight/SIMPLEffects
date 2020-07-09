using System;
using System.Text;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes

namespace SIMPLEffects
{
    public class Ramp : Effect
    {

        public Ramp(ushort duration, ushort basePhase, ushort minValue, ushort maxValue) : base(duration, basePhase, minValue, maxValue)
        {
        }

        protected override int getPhaseValue(float phase)
        {
            return minValue + (int)((maxValue - minValue) * phase);
        }
    }
}
