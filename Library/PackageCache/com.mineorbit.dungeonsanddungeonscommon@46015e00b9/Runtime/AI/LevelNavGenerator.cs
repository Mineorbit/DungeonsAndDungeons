using UnityEngine;
using UnityEngine.AI;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class LevelNavGenerator : MonoBehaviour
    {
        public NavMeshSurface navMeshSurface;

        private void Start()
        {
            if (navMeshSurface == null) navMeshSurface = GetComponent<NavMeshSurface>();
        }

        public void UpdateNavMesh()
        {
            Debug.Log("Building");
            var dynObjects = LevelManager.currentLevel.GetAllDynamicLevelObjects(false);
            foreach (var o in dynObjects) o.gameObject.SetActive(false);

            if (navMeshSurface == null) navMeshSurface = GetComponent<NavMeshSurface>();


            navMeshSurface.BuildNavMesh();

            Debug.Log("Built surface");

            foreach (var o in dynObjects) o.gameObject.SetActive(true);
        }
    }
}