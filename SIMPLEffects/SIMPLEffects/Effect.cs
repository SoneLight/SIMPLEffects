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

    public class RunningEventArgs : EventArgs
    {
        public ushort running;
    }

    public abstract class Effect
    {
        public static int REFRESHINTERVAL = 100;
        public static int VALUESCOUNT = 8;

        public event EventHandler<EffectValueEventArgs> ValuesChanged;
        public event EventHandler<RunningEventArgs> RunningChanged;

        private static Stopwatch stopwatch = null;

        protected float phase;
        protected int maxValue;
        protected int minValue;
        protected long duration;
        protected float width;

        private bool running = false;

        public bool IsRunning
        {
            protected set
            {
                running = value;

                var args = new RunningEventArgs();
                args.running = (ushort)(value ? 1 : 0);

                if (RunningChanged != null)
                    RunningChanged(this, args);
            }
            get 
            {
                return running;
            }
            
        }

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

        public Effect()
        {

        }

        public void setMaxValue(ushort maxValue)
        {
            this.maxValue = maxValue;
        }

        public void setMinValue(ushort minValue)
        {
            this.minValue = minValue;
        }

        public void setPhase(ushort phase)
        {
            this.phase = (phase / 65535.0f);
        }

        public void setDuration(ushort duration)
        {
            this.duration = duration;
        }

        public void setWidth(ushort width)
        {
            this.width = (width / 65535.0f);
        }

        public void Start()
        {
            if(running)
                return;

            timer.Reset(REFRESHINTERVAL, REFRESHINTERVAL);

            running = true;
        }

        public void Stop()
        {
            if (!running)
                return;

            timer.Stop();

            running = false;
        }

        public virtual void Initialize(ushort phase, ushort duration, ushort maxValue, ushort minValue, ushort width)
        {
            this.phase = (phase / 65535.0f);
            this.duration = duration*10;
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.width = (width / 65535.0f);
            timer = new CTimer(refresh, null, Timeout.Infinite);

            //CrestronConsole.PrintLine("phase: " + this.phase + ", duration: " + this.duration + ", maxval: " + this.maxValue + ", minval: " + this.minValue + ", width: " + this.width);
        }

        protected void refresh(object obj)
        {
            EffectValueEventArgs data = new EffectValueEventArgs();
            data.values = new ushort[VALUESCOUNT];            

            float valuephase = phase + (getTime() % duration) / (float)duration;

            for (int i = 0; i < VALUESCOUNT; i++)
            {
                data.values[i] = (ushort)(getPhaseValue(valuephase + i * (1.0f / VALUESCOUNT)));
            }
            //CrestronConsole.PrintLine("refresh. time " + getTime() + ", phase "  + valuephase);

            if (ValuesChanged != null)
            {
                ValuesChanged(this, data);
            }
        }

        protected abstract int getPhaseValue(float phase);

        
    }
}