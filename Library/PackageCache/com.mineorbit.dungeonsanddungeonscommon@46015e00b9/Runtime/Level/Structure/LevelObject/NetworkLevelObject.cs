using System;
using System.Collections.Generic;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class NetworkLevelObject : LevelObject
    {
        public LevelObjectNetworkHandler levelObjectNetworkHandler;

        private readonly Queue<Action> todo = new Queue<Action>();


        public virtual void FixedUpdate()
        {
            //this needs a safety later on to stop stupid behaviour
            if (todo.Count > 0)
                todo.Dequeue().Invoke();
        }


        public override void OnInit()
        {
            base.OnInit();

            levelObjectNetworkHandler = GetComponent<LevelObjectNetworkHandler>();
            //
            //if(levelObjectNetworkHandler == null) 
            //levelObjectNetworkHandler = gameObject.AddComponent<LevelObjectNetworkHandler>();
        }

        //This marks a message for transport through network
        public void Invoke<T>(Action<T> a, T argument, bool allowLocal = false)
        {
            if (allowLocal || Level.instantiateType == Level.InstantiateType.Play ||
                Level.instantiateType == Level.InstantiateType.Test) a.DynamicInvoke(argument);
            if (levelObjectNetworkHandler != null && levelObjectNetworkHandler.enabled)
                if (levelObjectNetworkHandler.identified)
                    levelObjectNetworkHandler.SendAction(a.Method.Name,
                        LevelObjectNetworkHandler.ActionParam.From(argument));
                else
                    todo.Enqueue(() => { Invoke(a, argument); });
        }

        public void Invoke(Action a, bool allowLocal = false)
        {
            if (allowLocal || Level.instantiateType == Level.InstantiateType.Play ||
                Level.instantiateType == Level.InstantiateType.Test) a.DynamicInvoke();
            if (levelObjectNetworkHandler != null && levelObjectNetworkHandler.enabled)
                if (levelObjectNetworkHandler.identified)
                    levelObjectNetworkHandler.SendAction(a.Method.Name);
                else
                    todo.Enqueue(() => { Invoke(a); });
        }
        /*

        public void Invoke(Action a)
        {
            MethodInfo methodInfo = a.Method;
            if (this.enabled) a.Invoke();
            if (h != null) h.Marshall(a);
        }*/
    }
}