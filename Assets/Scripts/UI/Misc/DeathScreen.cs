using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeathScreen : Openable
{
    public static DeathScreen instance;

    //transform of the info text
    public Transform content;
    public TextMeshProUGUI infoTextField;
    public UnityEvent openEvent;
    private UIAnimation animationInfoText;

    private UIAnimation animationScreen;

    //Helps with blocking UI
    private GraphicRaycaster rc;


    private void Start()
    {
        if (instance != null) Destroy(this);
        instance = this;
        content = transform.Find("Screen").Find("Content");
        rc = transform.GetComponent<GraphicRaycaster>();

        var screenOpenedEvent = new UnityEvent();
        var screenClosedEvent = new UnityEvent();
        var contentOpenedEvent = new UnityEvent();
        var contentClosedEvent = new UnityEvent();


        screenOpenedEvent.AddListener(OpenContent);
        contentOpenedEvent.AddListener(FinishOpen);
        contentClosedEvent.AddListener(CloseScreen);
        screenClosedEvent.AddListener(FinishClose);

        animationScreen = new Fade();
        animationScreen.target = transform;
        animationInfoText = new Fade();
        animationInfoText.target = content;
        animationScreen.InEndedEvent = screenOpenedEvent;
        animationScreen.OutEndedEvent = screenClosedEvent;
        animationInfoText.InEndedEvent = contentOpenedEvent;
        animationInfoText.OutEndedEvent = contentClosedEvent;
    }

    private void OpenContent()
    {
    }

    private void CloseScreen()
    {
        animationScreen.Play();
    }

    private void FinishOpen()
    {
        openEvent.Invoke();

        Finished = true;
    }

    private void FinishClose()
    {
        Finished = true;
    }

    public override void OnOpen()
    {
        animationScreen.Play();
    }

    public override void OnClose()
    {
        animationInfoText.Play();
    }
}