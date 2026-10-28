using System;
using System.Linq;
using Core.Invaders;
using UnityEngine;

namespace Core.Abilities
{
    [Serializable]
    public class AOEAbilityTargeting : AbilityTargeting
    {
        [SerializeField] private GameObject _aoePrefab;
        [SerializeField] private float _aoeRadius = 1f;
        [SerializeField] private LayerMask _aoeLayerMask = 1;
        
        private GameObject _aoeInstance;
        
        public override void Start(AbilitySettings abilitySettings, TargetingProcessor targetingProcessor)
        {
            _abilitySettings = abilitySettings;
            _targetingProcessor = targetingProcessor;
            
            _isTargeting = true;
            if (_aoePrefab != null)
                _aoeInstance = UnityEngine.Object.Instantiate(_aoePrefab);

            _targetingProcessor.SetTargeting(this);
            _targetingProcessor.Clicked += OnClick;
        }

        public override void Update()
        {
            if(!_isTargeting || !_aoeInstance)
                return;

            if (!_targetingProcessor.Camera)
                return;
            
            var ray = _targetingProcessor.Camera.ScreenPointToRay(Input.mousePosition);
            var mouseWorldPos = Physics.Raycast(ray, out var hit, 100f, _aoeLayerMask) ? hit.point : Vector3.zero;
            _aoeInstance.transform.position = mouseWorldPos;
        }

        public override void Stop()
        {
            _isTargeting = false;

            _targetingProcessor.RemoveTargeting();
            
            if(_aoeInstance)
                UnityEngine.Object.Destroy(_aoeInstance);

            _aoeInstance = null;
            _targetingProcessor.Clicked -= OnClick;
        }

        private void OnClick(RaycastHit hit)
        {
            if(!_isTargeting)
                return;

            var targets = Physics.OverlapSphere(hit.point, _aoeRadius)
                .Select(o => o.gameObject.GetComponent<InvaderLink>())
                .OfType<InvaderLink>();
            
            foreach (var target in targets)
                _abilitySettings.Execute(_targetingProcessor, target);

            Stop();
        }
    }
}