using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public float Target = 1;
    Slider s;
    float error = 0.01f;
    float MaxSpeed = 4;
    public float Speed = 2f;
    bool newVal = false;
    void Start()
    {
        s = this.transform.GetComponent<Slider>();
    }
    void Update()
    {
        if(newVal)
        {
        setValue();
        }

    }
    public void setBar(float t)
    {
        newVal = true;
        Target = t;
    }
    void setValue()
    {
            if(Mathf.Abs(Target-s.value)<error)
            {
                s.value = Target;
                newVal = false;
            }else
            {
                s.value = Mathf.Lerp(s.value,Target,1/(MaxSpeed-Speed));
            }
    }
}
