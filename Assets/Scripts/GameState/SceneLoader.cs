using Assets.Scripts.GameState.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameState
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded)
        {
            Debug.Log($"Start loading {nextScene} scene");
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            var waitNextSceneOperation = SceneManager.LoadSceneAsync(nextScene);

            while (waitNextSceneOperation.isDone)
                yield return null;

            Debug.Log($"Scene {nextScene} has been loaded");
            onLoaded?.Invoke();
        }
    }
}