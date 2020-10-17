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
    public Button openButton;
    bool open = false;


    Vector2 lastPosition;
    Vector2 lastObjectPosition;
    Vector3 targetPosition;
    void Start()
    {

        targetPosition = transform.position;
        //mainTransform.offsetMin = new Vector2(0,0);
        //mainTransform.offsetMax = new Vector2(0,0);
        openButton.onClick.AddListener(Click);
    }
    public void UpdateElement(LevelData.LevelMetaData data)
    {

    }
    public void Click() 
    {
        Debug.Log("Testtest");
        if(open)
        {
            
            //Set(1);
        }else
        {
            //Set(0);
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
    void Update()
    {
        updatePosition();
    }
    void updatePosition()
    {
        Vector2 offset;
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            lastObjectPosition = transform.position;
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastPosition;
            targetPosition = new Vector3(lastObjectPosition.x + offset.x, lastObjectPosition.y + offset.y, 0);
        }
        transform.position = (transform.position + targetPosition) / 2;
    }

}
