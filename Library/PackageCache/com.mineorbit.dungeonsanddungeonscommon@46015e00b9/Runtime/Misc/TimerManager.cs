using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager instance;

        private List<Timer> currentTimers;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else Setup();
        }


        // Update is called once per frame
        private void Update()
        {
        }

        public static Timer StartTimer(float t, Action a)
        {
            return StartTimer(new Timer(t, a));
        }

        public static bool isRunning(Timer t)
        {
            return instance.currentTimers.Contains(t);
        }

        public static Timer StartTimer(Timer timer)
        {
            if (!instance.currentTimers.Contains(timer))
            {
                instance.currentTimers.Add(timer);
                instance.StartCoroutine(timer.Coroutine);
            }

            return timer;
        }

        public static void StopTimer(Timer timer)
        {
            if (timer == null) return;
            if (instance.currentTimers.Contains(timer))
            {
                instance.StopCoroutine(timer.Coroutine);
                instance.currentTimers.Remove(timer);
            }
        }


        private void Setup()
        {
            instance = this;
            currentTimers = new List<Timer>();
        }

        public class Timer
        {
            public IEnumerator Coroutine;

            public Timer(float time, Action action)
            {
                Coroutine = GenerateCoroutine(time, action);
            }

            public IEnumerator GenerateCoroutine(float time, Action method)
            {
                yield return new WaitForSeconds(time);

                method();
                instance.currentTimers.Remove(this);
            }
        }
    }
}