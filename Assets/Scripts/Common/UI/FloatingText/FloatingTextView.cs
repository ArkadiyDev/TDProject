using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Common.UI.FloatingText
{
    public class FloatingTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _speed;
        [SerializeField] private float _duration;
        
        private Action _onEndAnimation;
        private Coroutine _coroutine;
        
        public void Set(Vector3 position, string text)
        {
            transform.position = position;
            _text.SetText(text);
        }

        public void StartAnimation(Transform cameraTransform, Action onEndAnimation)
        {
            _onEndAnimation = onEndAnimation;

            _coroutine = StartCoroutine(FloatingAnimation(cameraTransform));
        }

        public void StopAnimation()
        {
            if(_coroutine == null)
                return;
            
            StopCoroutine(_coroutine);
            _onEndAnimation?.Invoke();
        }

        private IEnumerator FloatingAnimation(Transform cameraTransform)
        {
            var timer = 0f;

            while (timer < _duration)
            {
                if (_speed > 0)
                    transform.position += cameraTransform.up * _speed * Time.deltaTime;
                
                transform.rotation = cameraTransform.rotation;
                
                timer += Time.deltaTime;
                
                yield return null;
            }
            
            _onEndAnimation?.Invoke();
        }
    }
}