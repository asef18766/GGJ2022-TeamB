using UnityEngine;
using asef18766.Scripts.Wolf;

namespace skps2010.Scripts
{
    public class Bullet : MonoBehaviour
    {
        private float speed = 2;

        void Update()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                WolfManager.GetInstance().KillWolf();
            Destroy(gameObject);
        }
    }
}