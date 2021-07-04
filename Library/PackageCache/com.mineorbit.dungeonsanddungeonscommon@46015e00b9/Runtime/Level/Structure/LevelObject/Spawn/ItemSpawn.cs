using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class ItemSpawn : Spawn
    {
        public LevelObjectData itemToSpawn;

        public Vector3 spawnOffset;

        private readonly int maxSpawnCount = 1;

        private int spawnCount = 1;

        private GameObject spawnedItem;

        //Change to on remove

        public void OnDestroy()
        {
            RemoveSpawnedItem();
        }

        public override void OnStartRound()
        {
            Setup();
        }

        public override void OnEndRound()
        {
            SetCollider();
        }

        private void SetCollider()
        {
            var full_collider = Level.instantiateType == Level.InstantiateType.Play ||
                                Level.instantiateType == Level.InstantiateType.Test ||
                                Level.instantiateType == Level.InstantiateType.Online;
            GetComponent<Collider>().enabled = !full_collider;
            GetComponent<Collider>().isTrigger = full_collider;
        }

        private void Setup()
        {
            spawnCount = maxSpawnCount;
            SpawnItem();

            SetCollider();
        }


        private void RemoveSpawnedItem(bool physics = true)
        {
            Debug.Log("Trying to Remove");

            if (spawnedItem != null) LevelManager.currentLevel.RemoveDynamic(spawnedItem.GetComponent<Item>(), physics);
        }

        public override void OnInit()
        {
            base.OnInit();
            Setup();
        }

        public override void OnDeInit()
        {
            base.OnDeInit();
            SetCollider();
            if (spawnedItem != null && spawnedItem.GetComponent<Item>().isEquipped)
                RemoveSpawnedItem(false);
        }

        private Vector3 SpawnLocation()
        {
            return transform.position + spawnOffset;
        }

        private Quaternion SpawnRotation()
        {
            return transform.rotation;
        }

        public void SpawnItem()
        {
            if (spawnedItem == null)
                spawnedItem = LevelManager.currentLevel.AddDynamic(itemToSpawn, SpawnLocation(), SpawnRotation());
            else
                LevelManager.currentLevel.AddToDynamic(spawnedItem, SpawnLocation(), SpawnRotation());
        }
    }
}