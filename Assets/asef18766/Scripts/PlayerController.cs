using System;
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
        
        private Rigidbody2D _rb = null;
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
                throw new Exception("controller can not obtain rb");
        }

        private Vector2 MoveDir()
        {
            var mvDir = new Vector2();
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
            _rb.AddForce(MoveDir() * speed);
        }
    }
}