using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace asef18766.Scripts.Wolf
{
    public class WolfManager
    {
        #region data
        private GameObject _wolf = null;
        
        private static WolfManager _instance = null;
        private readonly Vector2[] _spawnBounds= {
            new Vector2(0,0), // left up
            new Vector2(0,0), // right down
        };

        public GameObject playerRef = null;

        private int respawnCount = 3;
        
        #endregion

        private WolfManager()
        {
            _wolf = Resources.Load<GameObject>("Assets/asef18766/Prefabs/player.prefab");
        }

        public static WolfManager GetInstance()
        {
            return _instance ??= new WolfManager();
        }

        private Vector2 RespawnLoc()
        {
            // searching for nice location until wall does not exists
            while (true)
            {
                var x = Random.Range(_spawnBounds[0].x, _spawnBounds[1].x);
                var y = Random.Range(_spawnBounds[0].y, _spawnBounds[1].y);

                Debug.DrawRay(new Vector2(x,y), Vector2.one, Color.yellow);
                Debug.Log("try search spawn location");
                
                var objs = Physics2D.RaycastAll(new Vector2(x, y), Vector2.one);
                var wallExists = objs.Any(obj => obj.collider.CompareTag("Wall"));
                if (!wallExists)
                    return new Vector2(x, y);
            }
        }

        public void SpawnWolf()
        {
            var loc = RespawnLoc();
            playerRef = Object.Instantiate(_wolf, loc, Quaternion.identity);
        }

        public void KillWolf()
        {
            respawnCount--;
            Object.Destroy(_wolf);
            if (respawnCount == 0)
            {
                Debug.LogWarning("game over, player failed");
                throw new NotImplementedException();    
            }
            SpawnWolf();
        }
    }
}