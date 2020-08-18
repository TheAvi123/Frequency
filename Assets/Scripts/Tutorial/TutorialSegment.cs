using TMPro;

using UnityEngine;

namespace Tutorial
{
	public class TutorialSegment : MonoBehaviour
	{
		private enum InputType {Tap, Flip, Dash, Delay};

		//Configuration Parameters
		[SerializeField] private float freezeDelay = 0f;
		[SerializeField] private InputType inputType = InputType.Tap;
		[SerializeField] private TextMeshProUGUI[] fadeTextArray = null;
		[SerializeField] private float spawnWavelengthDelay = 1f;
		

		//Internal Methods
		private void Start() {
			InitializeFadeTextColors();
		}

		private void InitializeFadeTextColors() {
			foreach (TextMeshProUGUI text in fadeTextArray) {
				//text.color = Color.clear;
			}
		}

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				TriggerFreezeTime();
				TutorialSegmentSpawner.sharedInstance.SetReadyToSpawn(true);
			}
		}

		private void TriggerFreezeTime() {
			TutorialManager tutorialManager = TutorialManager.sharedInstance;
			tutorialManager.FreezeTime(freezeDelay, this);
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
		
		//Public Methods
		public TextMeshProUGUI[] GetFadeTextArray() {
			return fadeTextArray;
		}

		public float GetSpawnWavelengthDelay() {
			return spawnWavelengthDelay;
		}
	}
}