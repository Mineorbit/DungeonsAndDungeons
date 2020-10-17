using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelElement : MonoBehaviour
{
    UIAnimation openingAnimation;
    float bot = 325f;
    float right = 200f;
    public RectMask2D mask;
    public RectTransform mainTransform;
    public RectTransform surfaceTransform;
    bool open = false;

    Button openButton;
    void Start()
    {

        //mainTransform.offsetMin = new Vector2(0,0);
        //mainTransform.offsetMax = new Vector2(0,0);
        openButton = transform.Find("Cover").GetComponent<Button>();
        openButton.onClick.AddListener(Click);
    }
    public void UpdateElement(LevelData.LevelMetaData data)
    {

    }
    public void Click() 
    {
        if(open)
        {
            
            Set(1);
        }else
        {
            Set(0);
        }
        open = !open;
    }
    void Set(float t)
    {
        if (t<0 || t> 1) return;
        float r =  -t* right;
        float b =  -t * bot;
        mask.padding = new Vector4(0,r,b,0);
        Canvas.ForceUpdateCanvases();
    }
   
}
