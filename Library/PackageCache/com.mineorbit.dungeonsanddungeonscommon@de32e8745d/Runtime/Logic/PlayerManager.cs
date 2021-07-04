using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class PlayerManager : MonoBehaviour
    {
        public static bool acceptInput = true;
        public static PlayerManager playerManager;

        public static int currentPlayerLocalId;

        public static Player currentPlayer;
        public PlayerController[] playerControllers;
        public Player[] players;


        public void Reset()
        {
            for (var i = 0; i < 4; i++) Remove(i);
        }

        public void Start()
        {
            if (playerManager != null) Destroy(this);
            playerManager = this;
            playerControllers = new PlayerController[4];
            players = new Player[4];
        }

        public GameObject GetPlayer(int id)
        {
            return players[id].gameObject;
        }

        public void StartRound()
        {
            foreach (var p in players) p.OnStartRound();
        }


        public void SetCurrentPlayer(int localId)
        {
            if (localId > 3 || localId < 0) return;
            if (playerControllers[localId] == null)
            {
                if (playerControllers[localId] != null)
                    playerControllers[localId] = GameObject.Find("Player" + localId).GetComponent<PlayerController>();

                if (players[localId] != null)
                    players[localId] = GameObject.Find("Player" + localId).GetComponent<Player>();
            }

            for (var i = 0; i < 4; i++)
                if (playerControllers[i] != null)
                    playerControllers[i].activated = localId == i;
            currentPlayerLocalId = localId;
            currentPlayer = players[currentPlayerLocalId];
        }


        public Vector3 GetSpawnLocation(int i)
        {
            var location = new Vector3(i * 8, 6, 0);
            if (LevelManager.currentLevel != null)
                if (LevelManager.currentLevel.spawn[i] != null)
                    location = LevelManager.currentLevel.spawn[i].transform.position + new Vector3(0, 0.25f, 0);
            return location;
        }


        public void Remove(int localId)
        {
            if (localId > 3 || localId < 0) return;
            if (players[localId] != null)
                if (players[localId].gameObject != null)

                    Destroy(players[localId].gameObject);
        }


        public static Player GetPlayerById(int localId)
        {
            return playerManager.players[localId];
        }

        public void Add(int freeLocalId, string name, bool local)
        {
            var position = GetSpawnLocation(freeLocalId);

            var playerLevelObjectData = Resources.Load("LevelObjectData/Entity/Player") as LevelObjectData;
            var loadTargetData = Resources.Load("LevelObjectData/LevelLoadTarget") as LevelObjectData;

            var playerGameObject = playerLevelObjectData.Create(position, new Quaternion(0, 0, 0, 0), transform);
            var loadTargetGameObject = loadTargetData.Create(position, new Quaternion(0, 0, 0, 0), null);

            var loadTarget = loadTargetGameObject.GetComponent<LevelLoadTarget>();
            var player = playerGameObject.GetComponent<Player>();
            loadTarget.mover.target = player.transform;
            player.loadTarget = loadTarget;
            player.enabled = true;
            var playerController = playerGameObject.GetComponent<PlayerController>();

            playerController.locallyControllable = local;

            player.playerName = name;
            player.localId = freeLocalId;
            players[freeLocalId] = player;
            playerControllers[freeLocalId] = playerController;
            SpawnPlayer(freeLocalId,position);
        }


        public void SetPlayerActive(int id, bool a)
        {
            if (playerControllers[id] != null)
            {
                playerControllers[id].activated = a;
            }
        }
        

        public static void ActivateAllPlayers()
        {
            for(int i = 0;i<4;i++) playerManager.SetPlayerActive(i,true);
        }
        
        public static void DeactivateAllPlayers()
        {
            if(playerManager != null)
            for (int i = 0; i < 4; i++)
            {
                playerManager.SetPlayerActive(i,false);
            }
        }
        
        public void DespawnPlayer(int localId)
        {
            if (localId > 3 || localId < 0) return;
            if (players[localId] != null)
                players[localId].Despawn();
        }

        public void SpawnPlayer(int localId, Vector3 location)
        {
            if (localId > 3 || localId < 0) return;

            if (players[localId] == null) return;

            Debug.Log("Spawning " + localId + " at " + location);
            MainCaller.Do(() => { players[localId].Spawn(location, new Quaternion(0, 0, 0, 0), true); });
            //Move to other class Player eventually
            //Noch HUD Aktivieren
        }

        public static int GetPlayerId(GameObject player)
        {
            if (player.tag == "Entity")
            {
                var p = player.GetComponent<Player>();
                if(p != null)
                {
                for (var i = 0; i < 4; i++)
                    if (p == playerManager.players[i])
                        return i;
                }
            }

            return -1;
        }
    }
}