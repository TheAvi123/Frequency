using UnityEngine;

namespace Tutorial
{
	public class TimeFreezer : MonoBehaviour
	{
		private enum InputType {Tap, Flip, Dash, Delay};

		//Configuration Parameters
		[Header("Input Parameters")]
		[SerializeField] InputType inputType = InputType.Tap;
		[SerializeField] float freezeLerpDuration = 0.5f;
		[SerializeField] float freezeDelay = 0f;
		
		//Internal Methods
		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				TriggerFreezeTime();
			}
		}

		private void TriggerFreezeTime() {
			TutorialManager tutorialManager = TutorialManager.sharedInstance;
			tutorialManager.FreezeTime(freezeDelay, freezeLerpDuration);
			switch (inputType) {
				case InputType.Tap:
					tutorialManager.WaitForTap();
					break;
				case InputType.Flip:
					tutorialManager.WaitForFlip();
					break;
				case InputType.Dash:
					tutorialManager.WaitForDash();
					break;
				case InputType.Delay:
					tutorialManager.WaitForDelay();
					break;
			}
		}
		
	}
}