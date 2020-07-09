using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace SIMPLEffects
{

    public class EffectValueEventArgs : EventArgs
    {
        public ushort[] values;
    }

    public delegate void EffectValueEventDelegrate(EffectValueEventArgs args);

    public class Effect
    {
        public static int REFRESHINTERVAL = 25;
        public static int VALUESCOUNT = 8;


        private static Stopwatch stopwatch = null;

        protected float basePhase;
        protected int maxValue;
        protected int minValue;
        protected long duration;
        protected CTimer timer;

        public static long getTime()
        {
            if (stopwatch == null)
            {
                stopwatch = Stopwatch.StartNew();
                return 0;
            }
            return stopwatch.ElapsedMilliseconds;
        }

        public Effect(ushort duration, ushort basePhase, ushort minValue, ushort maxValue)
        {
            this.basePhase = (basePhase / 65535);
            this.duration = duration;
            this.maxValue = maxValue;
            this.minValue = minValue;
            timer = new CTimer(refresh, null, REFRESHINTERVAL, REFRESHINTERVAL);
        }

        protected void refresh(object obj)
        {
            EffectValueEventArgs data = new EffectValueEventArgs();
            data.values = new ushort[VALUESCOUNT];            

            float phase = basePhase + (getTime() % duration) / duration;

            for (int i = 0; i < VALUESCOUNT; i++)
            {
                data.values[i] = (ushort)(getPhaseValue(phase + i * (1.0f / VALUESCOUNT)));
            }
        }

        protected virtual int getPhaseValue(float phase);

        
    }
}