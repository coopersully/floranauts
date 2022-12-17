using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interfaces
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private Animator cameraAnimator;

        public GameObject window;
        private static readonly int Shop = Animator.StringToHash("shop");
        private static readonly int Career = Animator.StringToHash("career");
        private static readonly int Play = Animator.StringToHash("play");
        private static readonly int Settings = Animator.StringToHash("settings");
        private static readonly int Exit = Animator.StringToHash("exit");

        public void TransitionMainAndSaveCurrentPrefs()
        {
            TransitionPlay();
            FindObjectOfType<SettingsMenu>().SavePrefsAll();
        }
        
        public void TransitionShop()
        {
            panelAnimator.SetTrigger(Shop);
            cameraAnimator.SetTrigger(Shop);
            AudioManager.Instance.ui.Select01();
        }

        
        public void TransitionCareer()
        {
            panelAnimator.SetTrigger(Career);
            cameraAnimator.SetTrigger(Career);
            AudioManager.Instance.ui.Select01();
        }

    
        public void TransitionPlay()
        {
            panelAnimator.SetTrigger(Play);
            cameraAnimator.SetTrigger(Play);
            AudioManager.Instance.ui.Select01();
        }
    
        public void TransitionSettings()
        {
            panelAnimator.SetTrigger(Settings);
            cameraAnimator.SetTrigger(Settings);
            AudioManager.Instance.ui.Select01();
        }

        public void TransitionExit()
        {
            panelAnimator.SetTrigger(Exit);
            cameraAnimator.SetTrigger(Exit);
            AudioManager.Instance.ui.Select01();
        }

        public void StartGame() => LoadingScreen.Instance.Load(1);
    }
}
