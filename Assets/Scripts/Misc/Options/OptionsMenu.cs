using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Openable
{
    public static OptionsMenu options;

    public Toggle fullScreenToggle;

    public GameObject[] playerStores;

    private Button backButton;

    private Vector2 res;

    private TMP_Dropdown resolutionSelector;
    private UIAnimation transition;

    public void Start()
    {
        if (options != null) Destroy(this);
        options = this;


        backButton = transform.Find("Scroll View").Find("Back").GetComponent<Button>();

        SetupRes();
        SetupFull();

        backButton.onClick.AddListener(Close);
        transition = new Fade();
        transition.target = transform;

        res = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if (res != new Vector2(Screen.width, Screen.height))
        {
            HandleResChange();
            res = new Vector2(Screen.width, Screen.height);
        }
    }

    private void SetupFull()
    {
        fullScreenToggle = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Disp").Find("Full")
            .GetComponent<Toggle>();
        fullScreenToggle.onValueChanged.AddListener(delegate { FullChange(); });
        var storedVal = PlayerPrefs.GetInt("FullScreen", -1);
        if (storedVal != 0)
            fullScreenToggle.isOn = true;
        else
            fullScreenToggle.isOn = false;
        FullChange();
    }


    private void SetupRes()
    {
        resolutionSelector = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Disp")
            .Find("Resolutions").GetComponent<TMP_Dropdown>();

        resolutionSelector.ClearOptions();
        var i = 0;
        var cur = Screen.currentResolution.ToString();
        foreach (var r in Screen.resolutions)
        {
            var t = new TMP_Dropdown.OptionData();
            t.text = r.ToString();
            resolutionSelector.options.Insert(i, t);

            if (r.ToString().Equals(cur)) resolutionSelector.value = i;

            i++;
        }

        resolutionSelector.onValueChanged.AddListener(delegate { ResolutionChange(); });
    }

    private void FullChange()
    {
        var set = fullScreenToggle.isOn;
        Screen.fullScreen = set;
        PlayerPrefs.SetInt("FullScreen", set ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void HandleResChange()
    {
        LevelList.UpdateDisplay();
    }

    private void ResolutionChange()
    {
        var r = resolutionSelector.value;
        if (r > Screen.resolutions.Length) return;
        Screen.SetResolution(Screen.resolutions[r].width, Screen.resolutions[r].height, true);
    }

    public override void OnOpen()
    {
        Debug.Log("Open");
        transition.Play();
        Finished = true;
    }

    public override void OnClose()
    {
        Debug.Log("Close");
        transition.Play();
        Finished = true;
    }
}