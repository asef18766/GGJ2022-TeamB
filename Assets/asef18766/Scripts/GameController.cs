using System;
using asef18766.Scripts.Wolf;
using skps2010.Scripts;
using UnityEngine;

namespace asef18766.Scripts
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            WolfManager.GetInstance().SpawnWolf();
            for (var i = 0; i != 5; i++)
                VillagerController.GetInstance().SpawnVillager();
        }
    }
}