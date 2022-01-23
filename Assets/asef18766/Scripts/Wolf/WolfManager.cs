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
            new Vector2(-4,4), // left right
            new Vector2(-5,5), // down up
        };

        public GameObject PlayerRef = null;

        private int _respawnCount = 3;
        
        #endregion

        private WolfManager()
        {
            _wolf = Resources.Load<GameObject>("Prefabs/Player");
            if (_wolf == null)
                throw new ApplicationException("can not obtain player prefab");
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
                var x = Random.Range(_spawnBounds[0].x, _spawnBounds[0].y);
                var y = Random.Range(_spawnBounds[1].x, _spawnBounds[1].y);

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
            Debug.LogWarning($"try to spawn wolf at ({loc.x}, {loc.y})");
            PlayerRef = Object.Instantiate(_wolf, loc, Quaternion.identity);
        }

        public void KillWolf()
        {
            _respawnCount--;
            Object.Destroy(PlayerRef);
            PlayerRef = null;
            if (_respawnCount == 0)
            {
                Debug.LogWarning("game over, player failed");
                throw new NotImplementedException();    
            }
            else
            {
                SpawnWolf();
            }
        }
    }
}