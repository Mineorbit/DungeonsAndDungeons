using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class FSM<T, U>
    where U : System.Enum
    where T : System.Enum, new()
{
    public string name;

    public T state;
    public Dictionary<Tuple<T, U>, Tuple<Action<U>, T>> transitions;
    public FSM()
    {
        transitions = new Dictionary<Tuple<T, U>, Tuple<Action<U>, T>>();
    }

    public void Move(U inputValue)
    {
        T oldState = state;
        Tuple<Action<U>, T> value;
        if (transitions.TryGetValue(new Tuple<T, U>(state,inputValue), out value))
        {
            state = value.Item2;

            UnityEngine.Debug.Log($"[{name}] {oldState} -> {inputValue} -> {state}");
            value.Item1(inputValue);

        }
        else
        {
            UnityEngine.Debug.Log($"[{name}] In State {state} existiert kein Übergang für {inputValue}");
        }
    }
}
