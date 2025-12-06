using System;
using UnityEngine;

namespace Core.Invaders
{
    public class InvaderView : MonoBehaviour
    {
        [SerializeField] private Transform _bodyTargetPoint;
        
        public Transform BodyTargetPoint => _bodyTargetPoint;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
