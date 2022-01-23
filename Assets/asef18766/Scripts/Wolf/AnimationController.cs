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
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.Play(_currentAnimationState);
            _wolf = WolfManager.GetInstance().PlayerRef.GetComponent<Wolf>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateDirection(Vector2 dir)
        {
            var nState = "";
            if (dir == Vector2.left)
            {
                nState = "walk_side";
                _spriteRenderer.flipX = false;
            }
            else if (dir == Vector2.right)
            {
                nState = "walk_side";
                _spriteRenderer.flipX = true;
            }
            else if (dir.y < 0)
                nState = "walk_front";
            else if (dir.y > 0)
                nState = "walk_back";
            else
            {
                var substring = _currentAnimationState.Substring(4, _currentAnimationState.Length - 4);
                nState = "idle" + substring;
                Debug.Log(nState);
            }
            
            if (nState == _currentAnimationState)
                return;
            
            if (_wolf.WolfMode && (nState[nState.Length -1] != '0'))
                nState += " 0";

            _currentAnimationState = nState;
            _animator.Play(_currentAnimationState);
        }
    }
}