using System;
using System.Collections;
using UnityEngine;

namespace Core.Invaders
{
    public class InvaderView : MonoBehaviour
    {
        public event Action MoveComplete;
        
        [SerializeField] private Transform _bodyTargetPoint;

        private Coroutine _moveCoroutine;

        public Transform BodyTargetPoint => _bodyTargetPoint;

        private void OnDisable()
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
        }

        public void MoveTo(Vector3 position, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime);

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveCoroutine(position, speed));
        }

        private IEnumerator MoveCoroutine(Vector3 targetPosition, float speed)
        {
            while (!Mathf.Approximately(Vector3.Distance(transform.position, targetPosition), 0))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;
            _moveCoroutine = null;

            MoveComplete?.Invoke();
        }
    }
}
