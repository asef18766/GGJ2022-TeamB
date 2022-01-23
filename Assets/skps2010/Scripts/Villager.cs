using UnityEngine;
using System;
using asef18766.Scripts.Wolf;
using MangoZone;
using Random = UnityEngine.Random;

namespace skps2010.Scripts
{
    public class Villager : MonoBehaviour
    {
        private int state = 0;
        private float speed = 5;
        private double walkTime;
        private Quaternion newRotation;
        private bool isTurning = false;
        private double cooldown = 0;
        private const double error = 1;
        public GameObject Bullet;
        public VisionSpan VisionSpan;
        private GameObject GetPlayer()
        {
            return WolfManager.GetInstance().PlayerRef;
        }

        private Vector3 BulletStartPoint()
        {
            return transform.position + transform.up * 1.5f;
        }

        public void Update()
        {
            if (cooldown > 0)
                cooldown -= Time.deltaTime;

            if (GetPlayer() != null && VisionSpan.IsInSight(GetPlayer().transform.position))
            {
                state = 1;
            }
            else
            {
                state = 0;
            }

            // 亂走模式
            if (state == 0)
            {
                if (isTurning) // 轉
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, speed * 100 * Time.deltaTime);
                    if (Math.Abs(Quaternion.Angle(transform.rotation, newRotation)) < error)
                    {
                        isTurning = false;
                        walkTime = Random.Range(1, 3);
                    }
                }
                else //往前
                {
                    transform.position += transform.up * speed * Time.deltaTime;
                    walkTime -= Time.deltaTime;
                    if (walkTime <= 0)
                    {
                        isTurning = true;
                        newRotation = Quaternion.AngleAxis(Random.Range(0, 359), Vector3.forward);
                    }
                }
            }
            else // 攻擊模式
            {
                var direction = GetPlayer().transform.position - transform.position;
                var angle = Mathf.Atan2(-direction.x, direction.y);
                var target_rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, speed * 100 * Time.deltaTime);
                if (Math.Abs(Quaternion.Angle(transform.rotation, target_rotation)) < error)
                {
                    if (cooldown <= 0)
                    {
                        Instantiate(Bullet, BulletStartPoint(), transform.rotation);
                        cooldown = 3;
                    }
                }

            }
        }

        public void Hurt()
        {
            VillagerController.GetInstance().KillVillager(gameObject);
        }
    }
}