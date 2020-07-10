using System;
using System.Text;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes

namespace SIMPLEffects
{
    public class SinFX : Effect
    {
        protected override int getPhaseValue(float phase)
        {
            int midvalue = (int)((maxValue + minValue) / 2.0f);
            if (phase < width)
                return midvalue + (int)((maxValue-minValue)*Math.Sin(phase*2*Math.PI)*0.5f);
            else
                return midvalue;
        }
    }
}
