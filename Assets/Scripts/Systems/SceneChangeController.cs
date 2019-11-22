using UnityEngine;

public class SceneChangeController : MonoBehaviour
{
    //Reference Variables
    public static SceneChangeController sharedInstance;
    private Animator fadeAnimator = null;
    
    //Internal Methods
    private void Awake() {
        SetSharedInstance();
        FindFadeAnimator();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }
    
    private void FindFadeAnimator() {
        fadeAnimator = GetComponent<Animator>();
        if (!fadeAnimator) {
            Debug.LogError("No Animator Component Found on SceneChangeController");
            enabled = false;
        }
    }

    private void OnSceneChange() {
        //Do Nothing
    }

    private void FadeComplete() {
        GameStateManager.sharedInstance.FadeComplete();
    }
    
    //Public Methods
    public void TriggerFadeAnimation() {
        fadeAnimator.SetTrigger("FadeSwitch");
    }
}
