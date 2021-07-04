using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class EnemySpawn : Spawn
    {
        public LevelObjectData EnemyToSpawn;

        public Vector3 spawnOffset;

        private readonly int maxSpawnCount = 1;

        private int spawnCount = 1;

        private GameObject spawnedEnemy;

        //Change to on remove

        public void OnDestroy()
        {
            RemoveSpawnedEnemy();
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
            SetCollider();
            SpawnEnemy();
        }


        private void RemoveSpawnedEnemy()
        {
            Debug.Log("Trying to Remove");

            if (spawnedEnemy != null)
                LevelManager.currentLevel.RemoveDynamic(spawnedEnemy.GetComponent<Enemy>(), false);
        }

        public override void OnInit()
        {
            base.OnInit();
            Setup();
        }

        public override void OnDeInit()
        {
            base.OnDeInit();
            RemoveSpawnedEnemy();
            SetCollider();
        }

        private Vector3 SpawnLocation()
        {
            return transform.position + spawnOffset;
        }

        private Quaternion SpawnRotation()
        {
            Debug.Log(" Set Rotation to " + transform.rotation.eulerAngles);
            return transform.rotation;
        }

        public void SpawnEnemy()
        {
            if (!(Level.instantiateType == Level.InstantiateType.Online))
            {
                if (spawnedEnemy == null)
                {
                    var spawnLocation = SpawnLocation();
                    var spawnRotation = SpawnRotation();
                    spawnedEnemy = LevelManager.currentLevel.AddDynamic(EnemyToSpawn, spawnLocation, spawnRotation);
                    spawnedEnemy.GetComponent<Entity>().Spawn(spawnLocation, spawnRotation, true);
                }
                else
                {
                    LevelManager.currentLevel.AddToDynamic(spawnedEnemy, SpawnLocation(), SpawnRotation());
                }
            }
        }
    }
}