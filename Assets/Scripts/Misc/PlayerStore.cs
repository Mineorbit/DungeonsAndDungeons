using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStore : MonoBehaviour
{
    static Queue<bool> q;

    public GameObject[] playerStores;

    public Option lobbyOption;

    public static void Set(bool active)
    {
        if(q == null) q = new Queue<bool>();
        q.Enqueue(active);
        Debug.Log(q);
    }

    void Start()
    {
        playerStores = new GameObject[4];
        int i = 0;
        foreach(Transform t in transform)
        {
            playerStores[i] = t.gameObject;
            i++;
        }
        if(lobbyOption != null)
        Set(!((bool) lobbyOption.Value));
    }

    // Update is called once per frame
    void Update()
    {
        if(q.Count>0)
        {
            bool b = q.Dequeue();
            playerStores[0].SetActive(b);
            playerStores[1].SetActive(b);
            playerStores[2].SetActive(b);
            playerStores[3].SetActive(b);
        }
    }
}
