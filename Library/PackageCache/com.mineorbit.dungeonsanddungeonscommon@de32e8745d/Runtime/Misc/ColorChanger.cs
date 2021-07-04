using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class ColorChanger : MonoBehaviour
    {
        public List<Material> m;
        private Renderer[] rend;

        private void Awake()
        {
            rend = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var r in rend)
                m.AddRange(r.materials);
        }

        public void SetColor(int i, Color c)
        {
            Debug.Log("Setting color to" + c);
            m[i].SetColor("_BaseColor", c);
        }

        public Color comp(Color color)
        {
            Color.RGBToHSV(color, out var H, out var S, out var V);
            var negativeH = (H + 0.5f) % 1f;
            var negativeColor = Color.HSVToRGB(negativeH, S, V);
            return negativeColor;
        }
    }
}