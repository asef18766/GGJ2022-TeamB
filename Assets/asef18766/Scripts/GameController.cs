using System;
using asef18766.Scripts.Wolf;
using UnityEngine;

namespace asef18766.Scripts
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            WolfManager.GetInstance().SpawnWolf();
        }
    }
}