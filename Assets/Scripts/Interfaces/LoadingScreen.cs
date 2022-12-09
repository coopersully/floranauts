using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interfaces
{
    [RequireComponent(typeof(Animator))]
    public class LoadingScreen : MonoBehaviour
    {
        public static LoadingScreen Instance;
        
        public static bool IsLoading;
        private Animator _animator;

        private static readonly int StartTrigger = Animator.StringToHash("Start");
        private static readonly int EndTrigger = Animator.StringToHash("End");
        private void Start() => _animator = GetComponent<Animator>();
        
        public void Load(int buildIndex) => StartCoroutine(LoadScene(buildIndex));

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private IEnumerator LoadScene(int buildIndex)
        {
            IsLoading = true;
            _animator.SetTrigger(StartTrigger);
            
            yield return new WaitForSeconds(0.75f); 
            
            /* Begin loading the new scene and hold
            the function until it is finished loading. */ 
            var loadOperation = SceneManager.LoadSceneAsync(buildIndex); 
            while (!loadOperation.isDone) yield return null;

            // Mandatory 0.25f seconds of wait time for camera adjustment
            yield return new WaitForSeconds(0.25f); 
            
            _animator.SetTrigger(EndTrigger);
            
            // Wait until animator is done for pausing to be allowed
            yield return new WaitForSeconds(1.0f); 
            IsLoading = false;
        }
        
    }
}