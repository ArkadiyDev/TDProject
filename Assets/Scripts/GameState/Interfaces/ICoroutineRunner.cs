using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameState.Interfaces
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}