using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Instantiable : ScriptableObject
    {
        public Object prefab;

        public virtual GameObject Create(Vector3 location, Transform parent)
        {
            return Create(location, new Quaternion(0, 0, 0, 0), parent);
        }

        public virtual GameObject Create(Vector3 location, Quaternion rotation, Transform parent)
        {
            GameObject g = null;
            try
            {
                g = Instantiate(prefab) as GameObject;
                if(parent != null)
                    g.transform.SetParent(parent);
                g.transform.position = location;
                g.transform.rotation = rotation;
            }
            catch (Exception e)
            {
                GameConsole.Log($"Error: {e}");
                DestroyImmediate(g);
                return null;
            }
            return g;
        }

        // For UI
        public virtual GameObject Create(Vector2 location, Transform parent)
        {
            var g = Instantiate(prefab) as GameObject;
            g.SetActive(true);
            
            if(parent != null)
                g.transform.SetParent(parent);
            var rt = g.GetComponent<RectTransform>();
            rt.offsetMax = location;
            rt.offsetMin = location;
            return g;
        }
    }
}