using UnityEngine;

namespace skps2010.Scripts.Villager
{
    public class Villager : MonoBehaviour
    {
        private int state = 0;
        private float speed = 5;
        public Rigidbody2D rb;
        private double straight_time;
        private Quaternion new_rotation;
        private bool is_turning = false;

        void Update()
        {
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

            }
        }
    }
}