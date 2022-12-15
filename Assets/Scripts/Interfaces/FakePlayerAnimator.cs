using System;
using UnityEngine;

namespace Interfaces
{
    [RequireComponent(typeof(Animator))]
    public class FakePlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        public string trigger;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(trigger);
        }
    }
}
