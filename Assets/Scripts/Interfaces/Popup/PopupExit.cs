using System.Collections;
using UnityEngine;

namespace Interfaces.Popup
{
    public class PopupExit : StateMachineBehaviour
    {
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject popup = animator.gameObject;
            popup.SetActive(false);
        }
        
    }
}
