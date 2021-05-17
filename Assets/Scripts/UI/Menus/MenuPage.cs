using UnityEngine;

public class MenuPage : Openable
{
    public int index;
    private UIAnimation transferAnimation;

    public override void Awake()
    {
        setupTransition();
        base.Awake();
    }

    public virtual void Start()
    {
    }

    public void setupTransition()
    {
        transferAnimation = new Fade();
        transferAnimation.target = transform;
    }

    public override void OnOpen()
    {
        Debug.Log("Playing animation: " + this);
        transferAnimation.Play();
        Finished = true;
    }

    public override void OnClose()
    {
        transferAnimation.Play();
        Finished = true;
    }
}