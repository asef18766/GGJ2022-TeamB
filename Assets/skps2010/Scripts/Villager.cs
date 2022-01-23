using UnityEngine;
using System;
using asef18766.Scripts.Wolf;
using MangoZone;
using Random = UnityEngine.Random;

namespace skps2010.Scripts
{
    public enum State
    {
        Wandering,
        Attacking,
        Died,
    }
    public class Villager : MonoBehaviour
    {
        private State state = State.Wandering;
        private float speed = 1;
        private double walkTime;
        private Quaternion newRotation;
        private bool isTurning = false;
        private double cooldown = 0;
        private const double error = 1;
        private AnimationController _animationController;
        public GameObject Bullet;
        public VisionSpan VisionSpan;
        private void Start()
        {
            _animationController = GetComponent<AnimationController>();
        }
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

            if (state != State.Died)
            {
                if (GetPlayer() != null && GetPlayer().GetComponent<Wolf>().WolfMode && VisionSpan.IsInSight(GetPlayer().transform.position))
                {
                    state = State.Attacking;
                }
                else
                {
                    state = State.Wandering;
                }
                _animationController.UpdateDirection(isTurning ? Vector2.zero : (Vector2)transform.up);
            }
            else
            {
                _animationController.Die();
            }

            // 亂走模式
            if (state == State.Wandering)
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
            else if (state == State.Attacking)// 攻擊模式
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
            else // 死了
            {

            }
        }

        public void Hurt()
        {
            VillagerController.GetInstance().KillVillager(gameObject);
            state = State.Died;
        }
    }
}