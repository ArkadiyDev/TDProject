using System.Collections;
using UnityEngine;

namespace GameState.Interfaces
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}