using System;
using Core.Invaders;
using UnityEngine;

namespace Core.Castles
{
    public class CastleView : MonoBehaviour
    {
        public event Action<Invader> InvaderEntered;
        
        public void Enter(Invader invader)
        {
            InvaderEntered?.Invoke(invader);
        }
    }
}
