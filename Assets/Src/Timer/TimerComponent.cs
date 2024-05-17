using System;
using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
namespace Dawn
{
    public class TimerComponent : GameFrameworkComponent
    {
        int timerId = 0;
        Dictionary<int, Timer> existTimers;
        Dictionary<int, Timer> addTimers;
        List<int> removeTimers;
        // Start is called before the first frame update
        void Start()
        {
            addTimers = new Dictionary<int, Timer>();
            existTimers = new Dictionary<int, Timer>();
            removeTimers = new List<int>();
        }

        // Update is called once per frame
        void Update()
        {
            if (removeTimers.Count > 0)
            {
                foreach (int id in removeTimers)
                {
                    TryDeleteTimer(id);
                }
                removeTimers.Clear();
            }
            if (addTimers.Count > 0)
            {
                foreach (KeyValuePair<int, Timer> kv in addTimers)
                {
                    existTimers.Add(kv.Key, kv.Value);
                }
                addTimers.Clear();
            }
            foreach (KeyValuePair<int, Timer> kv in existTimers)
            {
                kv.Value.Update();
                if (kv.Value.IsFinished())
                {
                    DeleteTimer(kv.Key);
                }
            }
        }
        public int CreateTimer(float duration, Action onEnd, Action<float> onUpdate = null, bool loop = false, TimeUnit timeUnit = TimeUnit.SECOND)
        {
            var timer = new Timer(duration, timeUnit, loop);
            if (onEnd != null)
            {
                timer.OnEnd += onEnd;
            }
            if (onUpdate != null)
            {
                timer.OnUpdate += onUpdate;
            }
            timerId++;
            addTimers.Add(timerId, timer);
            return timerId;
        }
        public void PauseTimer(int id)
        {
            Timer timer;
            var suc = existTimers.TryGetValue(id, out timer);
            if (suc)
            {
                timer.Pause();
            }
        }
        public void UnpauseTimer(int id)
        {
            Timer timer;
            var suc = existTimers.TryGetValue(id, out timer);
            if (suc)
            {
                timer.Unpause();
            }

        }
        public void PauseAllTimer()
        {
            foreach (KeyValuePair<int, Timer> kv in existTimers)
            {
                kv.Value.Pause();
            }
        }
        public void UnpauseAllTimer()
        {
            foreach (KeyValuePair<int, Timer> kv in existTimers)
            {
                kv.Value.Pause();
            }
        }
        public void DeleteTimer(int id)
        {
            removeTimers.Add(id);
        }

        void TryDeleteTimer(int id)
        {
            Timer timer;
            var suc = existTimers.TryGetValue(id, out timer);
            if (suc)
            {
                timer.Stop();
                existTimers.Remove(id);
            }
        }

        void OnDestroy()
        {
            addTimers.Clear();
            removeTimers.Clear();
            foreach (KeyValuePair<int, Timer> kv in existTimers)
            {
                kv.Value.Stop();
            }
            existTimers.Clear();
        }
    }
}

