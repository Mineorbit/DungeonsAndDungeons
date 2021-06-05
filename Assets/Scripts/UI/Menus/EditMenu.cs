using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;

public class EditMenu : MenuPage
{
    public Switcher switcher;

    public static LevelList currentLevelList;
    public override void Awake()
    {
        base.Awake();
        index = 3;
        switcher.A = transform.Find("LocalLevelList").GetComponent<LevelList>();
        switcher.B = transform.Find("NetLevelList").GetComponent<LevelList>();
        switcher.valueChanged.AddListener((x) => { currentLevelList = (LevelList) switcher.selected;});
    }


    public override void Start()
    {
        base.Start();
    }

    
    

    
}