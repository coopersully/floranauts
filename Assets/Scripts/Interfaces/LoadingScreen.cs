using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Load(int buildIndex)
        {
            StartCoroutine(LoadScene(buildIndex));
        }
        
        public void Load(int buildIndex, List<GameObject> objects)
        {
            StartCoroutine(LoadSceneAndCarryObjects(buildIndex, objects));
        }

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
            return LoadSceneAndCarryObjects(buildIndex, null);
        }
        
        private IEnumerator LoadSceneAndCarryObjects(int buildIndex, List<GameObject> objects)
        {
            if (objects != null)
            {
                foreach (var givenObject in objects) givenObject.transform.SetParent(transform);
            }
            
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
            
            if (objects != null)
            {
                foreach (var givenObject in objects) givenObject.transform.SetParent(null);
            }
        }
        
    }
}