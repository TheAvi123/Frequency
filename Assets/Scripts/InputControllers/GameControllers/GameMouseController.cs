using Player;

using UnityEngine;

namespace InputControllers.GameControllers {
    public class GameMouseController : MouseController
    {
        //Reference Variables 
        private PlayerWave player = null;   //Player Movement Component

        private new void Awake() {
            base.Awake();
            FindPlayer();
        }

        private void FindPlayer() {
            player = FindObjectOfType<PlayerWave>();
            if (!player) {
                Debug.LogError("No Player Object Found");
                gameObject.SetActive(false);    //Disable Object
            }
        }

        //Input Actions
        protected override void TapAction() {
            player.Flip();
        }

        protected override void LeftSwipeAction() {
            float inputDistanceY = endPos.y - startPos.y;    //Vertical Swipe Distance
            if (inputDistanceY > 0) {
                //Player Swiped Up
                upSwipeAction();
            } else {
                //Player Swiped Down
                downSwipeAction();
            }
        }

        protected override void RightSwipeAction() {
            float inputDistanceY = endPos.y - startPos.y;    //Vertical Swipe Distance
            if (inputDistanceY > 0) {
                //Player Swiped Up
                upSwipeAction();
            } else {
                //Player Swiped Down
                downSwipeAction();
            }
        }

        protected override void UpSwipeAction() {
            player.Dash();
        }

        protected override void DownSwipeAction() {
            player.Delay();
        }
    }
}
