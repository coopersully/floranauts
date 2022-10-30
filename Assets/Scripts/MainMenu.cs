using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private static readonly int Main = Animator.StringToHash("main");
    private static readonly int Play = Animator.StringToHash("play");
    private static readonly int Settings = Animator.StringToHash("settings");

    public void TransitionMain()
    {
        animator.SetTrigger(Main);
        AudioManager.Instance.ui.Select01();
    }
    
    public void TransitionPlay()
    {
        animator.SetTrigger(Play);
        AudioManager.Instance.ui.Select01();
    }
    
    public void TransitionSettings()
    {
        animator.SetTrigger(Settings);
        AudioManager.Instance.ui.Select01();
    }
}
