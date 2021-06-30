using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public virtual void Reset()
    {
    }

    public virtual void Start()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    public virtual void performAction()
    {
    }

    public virtual void performAction(GameManager.GameAction g)
    {
        GameConsole.Log($"[Manager] Performing {g}");
    }
}