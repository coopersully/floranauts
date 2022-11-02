using Audio;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private Animator cameraAnimator;
    
    private static readonly int Main = Animator.StringToHash("main");
    private static readonly int Play = Animator.StringToHash("play");
    private static readonly int Settings = Animator.StringToHash("settings");

    public void TransitionMain()
    {
        panelAnimator.SetTrigger(Main);
        cameraAnimator.SetTrigger(Main);
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
}
