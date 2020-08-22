using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class FSM 
{
    string name;
    int currentState;
    int alphabetSize;
    int stateSize;
    public int[][] translationMatrix;
    public Action<int>[] stateOutput;
    public FSM(string FSMname, int[][] stateTranslationMatrix, Action<int>[] stateOutputs)
    {
        name = FSMname;
        if(stateTranslationMatrix.Length!=stateOutputs.Length)
        {
            UnityEngine.Debug.Log($"{name} FSM config wrong States: {stateTranslationMatrix.Length} Stateoutputs: {stateOutputs.Length}");
        }
        currentState = 0;
        translationMatrix = stateTranslationMatrix;
        stateSize = stateTranslationMatrix.Length;
        alphabetSize = stateTranslationMatrix[0].Length;
        stateOutput = stateOutputs;
    }
    public void Move(int input)
    {
        if(input >= alphabetSize)
        {
            UnityEngine.Debug.Log($"Input {input} ist für FSM {name} nicht erlaubt");
            return;
        }
        int newState = translationMatrix[currentState][input];
        if(newState>= stateSize || newState == -1)
        {

            UnityEngine.Debug.Log($"State {newState} ist für FSM {name} nicht definiert");
            return;
        }
        currentState = newState;
        stateOutput[currentState](input);
    }
}
