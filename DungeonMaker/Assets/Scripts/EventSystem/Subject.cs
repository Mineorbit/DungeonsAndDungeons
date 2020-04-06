using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    List<Observer> observers;

    void Awake()
    {
        observers = new List<Observer>();
    }
    public void  attach(Observer o)
    {

    }
    public void dettach(Observer o)
    {
        
    }

    void notify()
    {

    }
}
