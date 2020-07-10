using System;
using System.Text;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes

namespace SIMPLEffects
{
    public class StepFX : Effect
    {
        float attack;
        float decay;

        public void Initialize(ushort phase, ushort duration, ushort maxValue, ushort minValue, ushort width, ushort attack, ushort decay)       
        {
            base.Initialize(phase, duration, maxValue, minValue, width);
            this.attack = (float)(attack / 65535.0f);
            this.decay = (float)(decay / 65535.0f);
        }

        public void setAttack(ushort attack)
        {
            this.attack = (attack / 65535.0f);
        }

        public void setDecay(ushort decay)
        {
            this.decay = (decay / 65535.0f);
        }

        protected override int getPhaseValue(float phase)
        {
            if (phase < attack)
                return minValue + (int)((maxValue - minValue) * (phase / attack));
            if (phase < attack + width)
                return maxValue;
            if (phase < attack+width+decay)
                return maxValue - (int)((maxValue - minValue) * ((phase-attack-width) / decay));
            else
                return minValue;
        }
    }
}
