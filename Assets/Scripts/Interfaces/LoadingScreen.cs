using System;
using System.Collections;
using System.Collections.Generic;
using Player;
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
        
        public void Load(int buildIndex, List<PlayerController> players, Action afterwards = null)
        {
            StartCoroutine(LoadSceneAndCarryPlayers(buildIndex, players, afterwards));
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
            return LoadSceneAndCarryPlayers(buildIndex, null);
        }
        
        private IEnumerator LoadSceneAndCarryPlayers(int buildIndex, List<PlayerController> players, Action afterwards = null)
        {
            IsLoading = true;
            _animator.SetTrigger(StartTrigger);

            yield return new WaitForSeconds(0.75f); 
            
            /* If players are being carried across scenes, make them
             all children of the Loading Screen so that they become persistent objects. */
            if (players != null)
            {
                foreach (var player in players) player.transform.SetParent(transform);
            }
            
            /* Begin loading the new scene and hold
            the function until it is finished loading. */ 
            var loadOperation = SceneManager.LoadSceneAsync(buildIndex); 
            while (!loadOperation.isDone) yield return null;

            // Mandatory 0.25f seconds of wait time for camera adjustment
            yield return new WaitForSeconds(0.25f); 
            
            _animator.SetTrigger(EndTrigger);
            
            /* If players are being carried across scenes, make them no longer
             all children of the Loading Screen and move them to the newly loaded scene. */
            if (players != null)
            {
                var newScene = SceneManager.GetActiveScene();
                foreach (var player in players)
                {
                    player.transform.SetParent(null);
                    SceneManager.MoveGameObjectToScene(player.gameObject, newScene);
                }
            }
            
            // Wait until animator is done for pausing to be allowed
            yield return new WaitForSeconds(1.0f); 
            IsLoading = false;
            
            // Complete 'afterwards' action
            if (afterwards == null) yield break;
            
            Debug.Log("Scene loaded! Performing afterwards function... " + afterwards.Method);
            afterwards();
        }
        
    }
}