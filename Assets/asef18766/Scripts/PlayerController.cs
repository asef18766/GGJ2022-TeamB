using System;
using asef18766.Scripts.Wolf;
using UnityEngine;

namespace asef18766.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private KeyCode up = KeyCode.W;
        [SerializeField] private KeyCode down = KeyCode.S;
        [SerializeField] private KeyCode left = KeyCode.A;
        [SerializeField] private KeyCode right = KeyCode.D;
        [SerializeField] private float speed = 1;

        public Action<Vector2> WalkingCallback = vector2 => { Debug.Log("start walking");};
        public Action<Vector2> StopCallback = vector2 => { Debug.Log("stop walking");};

        private AnimationController _animationController;
        
        private Rigidbody2D _rb = null;
        private void Start()
        {
            _animationController = GetComponent<AnimationController>();
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
                throw new Exception("controller can not obtain rb");
        }

        private Vector2 MoveDir()
        {
            var mvDir = new Vector2(0,0);
            if (Input.GetKey(up))
                mvDir += Vector2.up;
            if (Input.GetKey(down))
                mvDir += Vector2.down;
            if (Input.GetKey(left))
                mvDir += Vector2.left;
            if (Input.GetKey(right))
                mvDir += Vector2.right;
            return mvDir.normalized;
        }

        private void Update()
        {
            var mvdir = MoveDir();
            _animationController.UpdateDirection(mvdir);
            
            if (mvdir == Vector2.zero)
                StopCallback(Vector2.zero);
            else
            {
                _rb.AddForce(mvdir * speed);
                WalkingCallback(mvdir);
            }
        }
    }
}