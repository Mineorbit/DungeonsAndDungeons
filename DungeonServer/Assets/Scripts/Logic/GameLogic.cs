using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Player[] players;

    long levelId = 0;
    public static GameLogic current;
    public void Start()
    {
        if (current != null) Destroy(this);
        current = this;
    }
    public void prepareRound()
    {

    }
    public static void ClearRound()
    {
        if (GameLogic.current != null)
        {
            Destroy(GameLogic.current);
        }
    }
    public static void StartRound(Transform t)
    {
        t.gameObject.AddComponent<GameLogic>();
    }
    public void AddPlayer(int localId, Client c)
    {

    }
    public void RemovePlayer(int localId)
    {

    }
}
