using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FileStructureProfile", order = 1)]
    public class FileStructureProfile : ScriptableObject
    {
        public string fileStructureProfileName;
        public FileStructureProfile parent;
        public List<FileStructureProfile> subStructures;

        private readonly TaskCompletionSource<bool> listReady = new TaskCompletionSource<bool>();

        private void OnEnable()
        {
            EnterInParent();
        }

        private async void EnterInParent()
        {
            subStructures = new List<FileStructureProfile>();
            listReady.SetResult(true);
            if (parent != null)
            {
                await parent.listReady.Task;
                parent.subStructures.Add(this);
            }
        }

        public string GetPath()
        {
            if (parent == null)
                return Application.persistentDataPath + "/" + fileStructureProfileName + "/";
            return parent.GetPath() + fileStructureProfileName + "/";
        }
    }
}