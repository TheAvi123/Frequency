using System.Diagnostics.CodeAnalysis;

using UnityEngine;

namespace Systems {
    public class SceneChangeController : MonoBehaviour
    {
        public static SceneChangeController sharedInstance;
        
        //Reference Variables
        private Animator fadeAnimator = null;
        
        //Configuration Parameters
        private static readonly int FadeSwitch = Animator.StringToHash("FadeSwitch");

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

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnSceneChange() {
            //Do Nothing
        }    //Called From Singleton

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void FadeInComplete() {
            GameStateManager.sharedInstance.SetFadeComplete();
        }   //Called From Animation Event
    
        //Public Methods
        public void TriggerFadeAnimation() {
            fadeAnimator.SetTrigger(FadeSwitch);
        }
    }
}
