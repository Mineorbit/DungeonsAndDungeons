using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class MainCaller : MonoBehaviour
    {
        public static MainCaller instance;

        public static Queue<Action> todo = new Queue<Action>();

        private readonly int maxActions = 15;

        public void Awake()
        {
            if (instance != null) Destroy(this);
            instance = this;
        }

        private void Update()
        {
            var actions = maxActions;
            while (todo.Count > 0 && actions > 0)
            {
                var x = todo.Dequeue(); 
                if(x != null)
                x.Invoke();
                actions--;
            }
        }

        public static void Do(Action a)
        {
            todo.Enqueue(a);
        }

        public static void startCoroutine(IEnumerator c)
        {
            instance.StartCoroutine(c);
        }
    }
}