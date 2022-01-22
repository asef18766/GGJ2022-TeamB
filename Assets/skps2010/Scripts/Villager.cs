using UnityEngine;

namespace skps2010.Scripts
{
    public class Villager : MonoBehaviour
    {
        private int state = 1;
        private float speed = 5;
        public Rigidbody2D rb;
        private double straight_time;
        private Quaternion new_rotation;
        private bool is_turning = false;
        private double cooldown = 0;
        public Transform player_transform;
        public GameObject bullet;

        void Update()
        {
            if (cooldown > 0)
                cooldown -= Time.deltaTime;
            // 亂走模式
            if (state == 0)
            {
                if (is_turning) // 轉
                {
                    Debug.Log(new_rotation);
                    Debug.Log(transform.rotation);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, new_rotation, speed * 100 * Time.deltaTime);
                    if (transform.rotation == new_rotation)
                    {
                        is_turning = false;
                        straight_time = Random.Range(1, 3);
                    }
                }
                else //前
                {
                    transform.position += transform.up * speed * Time.deltaTime;
                    straight_time -= Time.deltaTime;
                    if (straight_time <= 0)
                    {
                        is_turning = true;
                        new_rotation = Quaternion.AngleAxis(Random.Range(0, 359), Vector3.forward);
                    }
                }
            }
            else // 攻擊模式
            {
                var direction = player_transform.position - transform.position;
                var angle = Mathf.Atan2(-direction.x, direction.y); 
                var target_rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, speed * 100 * Time.deltaTime);
                if (transform.rotation == target_rotation)
                {
                    if (cooldown <= 0)
                    {
                        Instantiate(bullet, transform.position, transform.rotation);
                        cooldown = 3;
                    }
                }

            }
        }
    }
}