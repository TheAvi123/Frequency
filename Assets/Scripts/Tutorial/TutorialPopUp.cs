using Statistics;

using UnityEngine;

namespace Tutorial
{
    public class TutorialPopUp : MonoBehaviour
    {
        //Reference Variables
        private GameObject popUp;
    
        //Internal Methods
        private void Awake() {
            FindPopUp();
        }

        private void FindPopUp() {
            Canvas[] canvasList = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (Canvas canvas in canvasList) {
                if (canvas.CompareTag("TutorialPopUp") && canvas.isActiveAndEnabled) {
                    popUp = canvas.gameObject;
                }
            }
        }

        private void Start() {
            CheckPopUpCondition();
        }

        private void CheckPopUpCondition() {
            if (StatsManager.sharedInstance.GetRunsCompleted() == 0) {
                popUp.SetActive(true);
            } else {
                popUp.SetActive(false);
            }
        }
        
        //Public Methods
        public void ClosePopUp() {
            popUp.SetActive(false);
        }
    }
}
