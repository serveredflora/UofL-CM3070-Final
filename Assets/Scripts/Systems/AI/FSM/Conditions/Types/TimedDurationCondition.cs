using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Conditions/TimedDuration", fileName = "TimedDurationCondition")]
    public class TimedDurationCondition : ConditionAsset
    {
        [Header("Settings")]
        [SerializeField]
        private bool OneShot;
        [SerializeField]
        private float Duration;

        public override bool Evaluate(IFiniteStateMachineStorage storage)
        {
            float time = Time.time;
            if (!storage.TryRead<TimedDurationStorage>(out TimedDurationStorage timedStorage))
            {
                timedStorage = new TimedDurationStorage();
                storage.Write<TimedDurationStorage>(timedStorage);
            }

            if (!timedStorage.TryEvaluateElapsed(this, time, out bool elapsed))
            {
                timedStorage.CreateNew(this, time);
                return false;
            }

            return elapsed;
        }

        private class TimedDurationStorage
        {
            private readonly Dictionary<TimedDurationCondition, float> Timers = new();

            public void CreateNew(TimedDurationCondition condition, float time)
            {
                Timers[condition] = time + condition.Duration;
            }

            public bool TryEvaluateElapsed(TimedDurationCondition condition, float time, out bool elapsed)
            {
                if (!Timers.TryGetValue(condition, out float elapseTime))
                {
                    elapsed = false;
                    return false;
                }

                elapsed = elapseTime <= time;
                if (elapsed && condition.OneShot)
                {
                    // Remove so that this only triggers once (one shot) 
                    Timers.Remove(condition);
                }

                return true;
            }
        }
    }
}