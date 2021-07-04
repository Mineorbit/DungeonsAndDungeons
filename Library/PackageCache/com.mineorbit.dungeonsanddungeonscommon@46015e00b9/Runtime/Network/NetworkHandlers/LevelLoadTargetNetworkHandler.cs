using Game;
using General;
using NetLevel;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class LevelLoadTargetNetworkHandler : LevelObjectNetworkHandler
    {

        public override void Awake()
        {
            disabled_observed = true;
            base.Awake();
        }
        
        
        [PacketBinding.Binding]
        public static void OnStreamChunk(Packet p)
        {
            StreamChunk streamChunk;
            if (p.Content.TryUnpack(out streamChunk))
                MainCaller.Do(() =>
                {
                    ChunkData c = streamChunk.ChunkData;
                    ChunkManager.LoadChunk(c, streamChunk.Immediate);
                });
        }

        private void StreamChunk(ActionParam chunkParam, bool immediate = false)
        {
            ChunkData toSend = (ChunkData) chunkParam.data;
            var streamChunk = new StreamChunk
            {
                ChunkData = toSend,
                Immediate = immediate
            };
            Marshall(streamChunk, TCP: true);
        }

        public override void SendAction(string actionName, ActionParam argument)
        {

            switch (actionName)
            {
                case "StreamChunkIntoCurrentLevelFrom":
                    StreamChunk(argument);
                    break;
                case "StreamChunkImmediateIntoCurrentLevelFrom":
                    StreamChunk(argument,immediate: true);
                    break;
                default:
                    base.SendAction(actionName, argument);
                    break;
            }
        }
    }
}