using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    Renderer[] rend;
    public List<Material> m;
    void Awake()
    {
        rend = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rend)
            m.AddRange(r.materials);
    }

    public void SetColor(int i, Color c)
    {
        Debug.Log("Setting color to"+c);
        m[i].SetColor("_BaseColor",c);
    }
    public Color comp(Color color)
    {
        Color.RGBToHSV(color, out float H, out float S, out float V);
        float negativeH = (H + 0.5f) % 1f;
        Color negativeColor = Color.HSVToRGB(negativeH, S, V);
        return negativeColor;
    }

}
