using System;
using UnityEngine;

namespace asef18766.Scripts.Wolf
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private string _currentAnimationState = "idle_front";
        
        private string[] _behaviours = {
            "idle",
            "walk"
        };
        
        private string[] _directions = {
            "side",
            "front",
            "back"
        };

        private Wolf _wolf = null;
        private SpriteRenderer _spriteRenderer = null;
        private static readonly int FaceDir = Animator.StringToHash("face_dir");
        private static readonly int IsMoving = Animator.StringToHash("is_moving");
        private static readonly int WolfMode = Animator.StringToHash("wolf_mode");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _wolf = WolfManager.GetInstance().PlayerRef.GetComponent<Wolf>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateDirection(Vector2 dir)
        {
            if (dir != Vector2.zero)
            {
                if (dir == Vector2.left)
                {
                    _animator.SetInteger(FaceDir, 2);
                    _spriteRenderer.flipX = false;
                }
                else if (dir == Vector2.right)
                {
                    _animator.SetInteger(FaceDir, 2);
                    _spriteRenderer.flipX = true;
                }
                else if (dir.y < 0)
                    _animator.SetInteger(FaceDir, 0);
                else
                    _animator.SetInteger(FaceDir, 1);
                _animator.SetBool(IsMoving, true);
            }
            else
                _animator.SetBool(IsMoving, false);
            
            _animator.SetBool(WolfMode, _wolf.WolfMode);
        }
    }
}