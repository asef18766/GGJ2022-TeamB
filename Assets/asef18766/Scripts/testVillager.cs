using asef18766.Scripts.Audio;
using UnityEngine;

namespace asef18766.Scripts
{
    public class testVillager : MonoBehaviour
    {
        public void Hurt()
        {
            Debug.Log("ouch");
        }

        private void Start()
        {
            //WolfManager.GetInstance().SpawnWolf();
            AudioManager.Instance.SetSoundLoop("toho");
            AudioManager.Instance.PlaySound("toho");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                AudioManager.Instance.PlaySound("nani");
        }
    }
}