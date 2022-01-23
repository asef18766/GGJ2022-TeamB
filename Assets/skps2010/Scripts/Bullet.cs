using UnityEngine;
using asef18766.Scripts.Wolf;

namespace skps2010.Scripts
{
    public class Bullet : MonoBehaviour
    {
        private float speed = 5;

        void Update()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
            WolfManager.GetInstance().KillWolf();
        }
    }
}