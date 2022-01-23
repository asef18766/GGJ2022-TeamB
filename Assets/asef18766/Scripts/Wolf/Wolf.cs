using System;
using System.Collections;
using System.Collections.Generic;
using asef18766.Scripts.Audio;
using Assets.hundo1018.Scripts;
using Assets.hundo1018.Scripts.Stage;
using UnityEngine;

namespace asef18766.Scripts.Wolf
{
    public class Wolf : MonoBehaviour
    {
        [SerializeField] private float attackCd = 0.5f;
        [SerializeField] private float attackRange = 1;
        [SerializeField] private KeyCode attackKey = KeyCode.Space;
        [SerializeField] private float chargeTime = 3;
        [SerializeField] private float wolfModeDuration = 3;
        private StageView _hud;
        
        public Action<float> UpdateHud = t => { Debug.Log($"update time to {t}"); };
        public Action<object> EndWolfMode = o => { Debug.Log("end of wolf mode event");};
        public Action<object> WolfAttack = o => { Debug.Log("wolf attack callback"); };
        public readonly List<Action<object>> StartWolfMode = new List<Action<object>>();

        private bool _canAttack = true;
        private bool _wolfMode = false;
        private bool _charging = false;
        private float _wolfModeRemainTime = 0;

        public bool WolfMode => _wolfMode;

        private IEnumerator _attack()
        {
            if (!_canAttack) yield break;
            Debug.Log("wolf attack");

            _canAttack = false;
            WolfAttack(null);
            AudioManager.Instance.PlaySound("attack");
            var position = transform.position;
            var objs = Physics2D.CircleCastAll(new Vector2(position.x, position.y), attackRange, Vector2.zero);
            foreach (var obj in objs)
            {
                if (!obj.collider.CompareTag("Villager")) continue;
                obj.collider.gameObject.SendMessage("Hurt");
            }
            yield return new WaitForSeconds(attackCd);
            _canAttack = true;
        }

        private IEnumerator _hurt()
        {
            WolfManager.GetInstance().KillWolf();
            yield break;
        }

        private IEnumerator _startCharge()
        {
            if (_charging) yield break;
            _charging = true;
            yield return new WaitForSeconds(chargeTime);
            if (_charging)
            {
                _wolfMode = true;
                Debug.LogWarning("enter wolf mode");
                yield return _wolfModeCountDown();
            }
            else
            {
                Debug.LogWarning("charge failed");
            }
        }

        private const float HudUpdateDuration = 0.5f;
        private IEnumerator _wolfModeCountDown()
        {
            foreach (var ev in StartWolfMode)
                ev(null);
            
            _wolfModeRemainTime = wolfModeDuration;
            do
            {
                UpdateHud(_wolfModeRemainTime);
                yield return new WaitForSeconds(HudUpdateDuration);
                _wolfModeRemainTime -= HudUpdateDuration;
            } while (_wolfModeRemainTime > 0);
            _wolfMode = false;
            _charging = false;
            EndWolfMode(null);
        }

        #region unity_event
        private void OnDrawGizmos()
        {
            // attack range display
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, attackRange);
        }

        private void Start()
        {
            _hud = FindObjectOfType<StageView>();
            StartWolfMode.Add(o => { Debug.Log("start of wolf mode event"); });
            UpdateHud = f =>
            {
                _hud.OnTimeUpdate(null, new TimeEventArgs(f, f/wolfModeDuration));
                AudioManager.Instance.PlaySound("night_ambiance_10s");
            };
            StartWolfMode.Add(o => {
                var args = new TimeEventArgs(wolfModeDuration, wolfModeDuration) {IsNight = true};
                _hud.OnStageChanged(null, args);
                AudioManager.Instance.PlaySound("night_began");
            });
            EndWolfMode = o =>
            {
                Debug.LogWarning("end of wolf mode");
                var args = new TimeEventArgs(0, wolfModeDuration) {IsNight = false};
                _hud.OnStageChanged(null, args);
                AudioManager.Instance.StopSound("night_ambiance_10s");
            };
            if (Camera.main is { })
            {
                Camera main;
                (main = Camera.main).transform.SetParent(transform);
                main.transform.localPosition = new Vector3(0, 0, -1);
            }
        }

        private void Update()
        {
            if (Input.GetKey(attackKey))
                StartCoroutine(_wolfMode ? _attack() : _startCharge());
            else
                _charging = false;
        }

        private void OnDestroy()
        {
            var args = new TimeEventArgs(0, wolfModeDuration) {IsNight = false};
            _hud.OnStageChanged(null, args);
            AudioManager.Instance.StopSound("night_ambiance_10s");
        }

        #endregion
    }
}