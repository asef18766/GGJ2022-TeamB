using System.Collections;
using UnityEngine;

namespace asef18766.Scripts.Wolf
{
    public class Wolf : MonoBehaviour
    {
        [SerializeField] private float attackCd = 0.5f;
        [SerializeField] private float attackRange = 1;
        [SerializeField] private KeyCode attackKey = KeyCode.Space;
        private bool _canAttack = true;
        private IEnumerator _attack()
        {
            if (!_canAttack) yield break;
            Debug.Log("wolf attack");

            _canAttack = false;

            var position = transform.position;
            var objs = Physics2D.CircleCastAll(new Vector2(position.x, position.y), attackRange, Vector2.zero);
            Gizmos.DrawSphere(position, attackRange);

            foreach (var obj in objs)
            {
                if(obj.collider.CompareTag("Villager"))
                    obj.collider.gameObject.SendMessage("_hurt");
            }
            yield return new WaitForSeconds(attackCd);
            _canAttack = true;
        }

        private IEnumerator _hurt()
        {
            WolfManager.GetInstance().KillWolf();
            yield break;
        }

        void Update()
        {
            if (Input.GetKeyDown(attackKey))
                StartCoroutine(_attack());
        }

    }
}