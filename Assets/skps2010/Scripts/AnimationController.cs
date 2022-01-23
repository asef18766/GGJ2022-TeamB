using System;
using UnityEngine;

namespace skps2010.Scripts
{
    public class AnimationController : MonoBehaviour
    {
        public GameObject Target;
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

        private SpriteRenderer _spriteRenderer = null;
        private void Start()
        {
            _animator = Target.GetComponent<Animator>();
            _animator.Play(_currentAnimationState);
            _spriteRenderer = Target.GetComponent<SpriteRenderer>();
        }
        public void Die()
        {
            Target.transform.eulerAngles = new Vector3(0, 0, 0);
            if (_currentAnimationState != "villager_die")
            {
                _currentAnimationState = "villager_die";
                _animator.Play(_currentAnimationState);
            }
        }

        public void UpdateDirection(Vector2 dir)
        {
            Target.transform.eulerAngles = new Vector3(0, 0, 0);
            var nState = "";

            if (dir.x == 0 && dir.y == 0)
            {
                var substring = _currentAnimationState.Substring(4, _currentAnimationState.Length - 4);
                nState = "idle" + substring;
            }
            else
            {
                var angle = Vector2.Angle(Vector2.up, dir);
                if (angle >= 45 && angle < 135)
                {
                    nState = "walk_side";
                    _spriteRenderer.flipX = dir.x > 0;
                }
                else if (angle >= 135 && angle <= 180)
                {
                    nState = "walk_front";
                }
                else
                    nState = "walk_back";
            }

            if (nState == _currentAnimationState)
                return;

            _currentAnimationState = nState;
            _animator.Play(_currentAnimationState);
        }
    }
}