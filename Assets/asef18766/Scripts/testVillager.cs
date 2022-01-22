using System.Collections;
using UnityEngine;

namespace asef18766.Scripts
{
    public class testVillager : MonoBehaviour
    {
        private IEnumerator _hurt()
        {
            Debug.Log("ouch");
            yield break;
        }
    }
}