using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelObjectData", order = 1)]
    public class LevelObjectData : Instantiable
    {
        public static List<LevelObjectData> all = new List<LevelObjectData>();
        public int uniqueLevelObjectId;

        public float granularity;

        public string levelObjectName;

        public bool dynamicInstantiable;

        public bool levelInstantiable;

        public bool ActivateWhenInactive;

        public bool Buildable;

        public Mesh cursorMesh;

        
        public Vector3 cursorScale;
        public Vector3 cursorOffset;
        public Vector3 cursorRotation;


        void AssignUniqueNumbers()
        {
            if (uniqueLevelObjectId == 0)
            {
#if UNITY_EDITOR
                uniqueLevelObjectId = GUID.Generate().GetHashCode();
#endif
            }
            
        }
        private void OnValidate()
        {
#if UNITY_EDITOR
            AssignUniqueNumbers();
            if (Buildable) levelInstantiable = true;

            if (cursorScale == Vector3.zero) cursorScale = new Vector3(1, 1, 1);
#endif
        }

        public void OnEnable()
        {
            UpdateMesh();
        }

        public static LevelObjectData[] GetAllBuildable()
        {
            var data = new List<LevelObjectData>(Resources.LoadAll<LevelObjectData>("LevelObjectData"));
            return data.FindAll(x => x.Buildable).ToArray();
        }

        public static Dictionary<int, LevelObjectData> GetAllByUniqueType()
        {
            var data = new List<LevelObjectData>(Resources.LoadAll<LevelObjectData>("LevelObjectData"));
            var dict = new Dictionary<int, LevelObjectData>();
            foreach (var objectData in data) dict.Add(objectData.uniqueLevelObjectId, objectData);
            return dict;
        }

        public static Dictionary<int, LevelObjectData> GetAllBuildableByUniqueType()
        {
            var data = new List<LevelObjectData>(Resources.LoadAll<LevelObjectData>("LevelObjectData"));
            data = data.FindAll(x => x.Buildable);
            var dict = new Dictionary<int, LevelObjectData>();
            foreach (var objectData in data) dict.Add(objectData.uniqueLevelObjectId, objectData);
            return dict;
        }


        public void UpdateMesh()
        {
            if(prefab != null)
            {
                GameObject g = null;
            try
            {
                GameConsole.Log($"Instantiating {levelObjectName}",false);
                g = Instantiate(prefab) as GameObject;
                Destroy(g.GetComponent<LevelObject>());
                g.SetActive(false);
            MeshFilter[] meshFilters = g.GetComponentsInChildren<MeshFilter>(includeInactive: true);
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }
            cursorMesh = new Mesh();
            cursorMesh.CombineMeshes(combine);
            }
            catch (Exception e)
            {
                if (Application.isPlaying)
                {
                  
                    Destroy(g);
                }
                else
                {
                    DestroyImmediate(g);  
                }
                
                Console.WriteLine(e);
                return;
            }
            Debug.Log("Removing "+g);
            if (Application.isPlaying)
            {
                Destroy(g);
            }
            else
            {
                DestroyImmediate(g);  
            }
            }
        }

        public Mesh GetMesh()
        {
            if (cursorMesh == null)
            {
                UpdateMesh();
            }
            return cursorMesh;
        }

        public override GameObject Create(Vector3 location, Quaternion rotation, Transform parent)
        {
            var g = base.Create(location, rotation, parent);
            var lO = g.GetComponent<LevelObject>();
            lO.levelObjectDataType = uniqueLevelObjectId;
            lO.OnInit();
            return g;
        }
    }
}