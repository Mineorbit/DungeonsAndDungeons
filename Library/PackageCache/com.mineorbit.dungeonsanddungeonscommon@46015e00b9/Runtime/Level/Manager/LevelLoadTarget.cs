using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetLevel;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class LevelLoadTarget : NetworkLevelObject
    {
        public enum LoadTargetMode
        {
            None,
            Near
        }

        public static LoadTargetMode loadTargetMode = LoadTargetMode.None;

        public LevelLoadTargetMover mover;

        private readonly List<Tuple<int, int>> loadedLocalChunks = new List<Tuple<int, int>>();


        public void Start()
        {
            LevelManager.levelClearEvent.AddListener(() => { loadedLocalChunks.Clear();});
        }

        //Overlap between load target zones is a problem
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(Level.instantiateType == Level.InstantiateType.Play || Level.instantiateType == Level.InstantiateType.Test)
            {
            EnableChunkAt(transform.position);
            EnableChunkAt(transform.position + transform.forward * 32);
            EnableChunkAt(transform.position - transform.forward * 32);
            EnableChunkAt(transform.position + transform.right * 32);
            EnableChunkAt(transform.position - transform.right * 32);
            }
        }

        private void EnableChunkAt(Vector3 position)
        {
            if (loadTargetMode == LoadTargetMode.Near) LoadNearChunk(position);
        }

        public void LoadNearChunk(Vector3 position, bool immediate = false)
        {
            if (LevelManager.currentLevel != null)
                if (!loadedLocalChunks.Contains(ChunkManager.GetChunkGridPosition(position)))
                {
                    var chunkData = ChunkManager.ChunkToData(ChunkManager.GetChunk(position, true));
                    if (chunkData != null)
                    {
                        if (immediate)
                            Invoke(StreamChunkImmediateIntoCurrentLevelFrom, chunkData);
                        else
                            Invoke(StreamChunkIntoCurrentLevelFrom, chunkData);

                        loadedLocalChunks.Add(ChunkManager.GetChunkGridPosition(position));
                    }
                }
        }


        IEnumerator WaitRoutine(long cid,Action finishAction)
        {
            bool v = false;
            v = ChunkManager.ChunkLoaded(cid);
            while (!v)
            {
                v = ChunkManager.ChunkLoaded(cid);
                yield return new WaitForEndOfFrame();
            }
            mover.follow = true;
            MainCaller.Do(finishAction);  
        }
        
        public void WaitForChunkLoaded(Vector3 position, Action finishAction)
        {
            MainCaller.Do(() =>
            {
                mover.follow = false;
                transform.position = position;
                long chunkId = ChunkManager.GetChunkID(ChunkManager.GetChunkGridPosition(position));
                StartCoroutine(WaitRoutine(chunkId,finishAction));
            });
        }

        public void StreamChunkImmediateIntoCurrentLevelFrom(ChunkData chunkData)
        {
            GameConsole.Log("Streaming Chunk immediately " + chunkData.ChunkId);
            ChunkManager.LoadChunk(chunkData, true);
        }

        public void StreamChunkIntoCurrentLevelFrom(ChunkData chunkData)
        {
            Debug.Log("Streaming Chunk " + chunkData.ChunkId);
            ChunkManager.LoadChunk(chunkData);
        }


        private void DisableChunkAt(Vector3 position)
        {
        }
    }
}