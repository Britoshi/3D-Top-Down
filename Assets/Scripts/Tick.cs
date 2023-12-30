using System;
using System.Linq;
using System.Collections.Generic;

namespace Game
{
    public class Tick
    {
        public static Tick Instance { private set; get; }
        public event Action OnTick;
        public const float TICK_INTERVAL = .2f; // .2 second interval, but you can change this
        private double lastTickTime;
        public double time;

        public static float DeltaTime { get; private set; }

        /// <summary>
        /// This should only be called from OnTick Functions
        /// </summary>
        public static double TrueDeltaTime => Instance.time - Instance.lastTickTime;
        public void Start()
        {
            Instance = this;
            time = 0;
        }
        public void Update(double delta)
        {
            time += delta;
            if (time - lastTickTime > TICK_INTERVAL)
            {
                // Raise the OnTick event
                OnTick?.Invoke();
                lastTickTime = time;
                DeltaTime = (float)(time - lastTickTime);
            }
        }

        public static void AddFunction(Action function)
        {
            Instance.OnTick += function;
        }

        public static void RemoveFunction(Action function)
        {
            Instance.OnTick -= function;
        }

    }
}