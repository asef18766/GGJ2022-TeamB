using System;
using System.Linq;
using UnityEngine;
using asef18766.Scripts.Wolf;
using System.Collections;
using Random = UnityEngine.Random;

namespace skps2010.Scripts
{
    public class VillagerController : MonoBehaviour
    {
        private readonly Vector2[] _spawnBounds= {
            new Vector2(-4,4), // left right
            new Vector2(-5,5), // down up
        };
        private ArrayList villagers = new ArrayList();
        public GameObject Villager;
        private void Start()
        {
            WolfManager.GetInstance().SpawnWolf();
            SpawnVillager();
            SpawnVillager();
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

        public void SpawnVillager()
        {
            var loc = RespawnLoc();
            Debug.LogWarning($"try to spawn a villager at ({loc.x}, {loc.y})");
            var villagerRef = Instantiate(Villager, loc, Quaternion.identity);
            villagers.Add(villagerRef);
        }

        public void KillVillager(GameObject villager)
        {
            villagers.Remove(villager);
            Destroy(villager);
        }

        public int VillagerCount()
        {
            return villagers.Count;
        }
    }
}