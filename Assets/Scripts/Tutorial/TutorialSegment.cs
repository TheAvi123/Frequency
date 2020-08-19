using System.Collections;

using TMPro;

using UnityEngine;

namespace Tutorial
{
	public class TutorialSegment : MonoBehaviour
	{
		private enum InputType {Tap, Flip, Dash, Delay};

		//Configuration Parameters
		[Header("Spawn Variables")]
		[SerializeField] float spawnWavelengthDelay = 1f;

		[Header("Input Parameters")]
		[SerializeField] InputType inputType = InputType.Tap;
		[SerializeField] float freezeDelay = 0f;
		
		[Header("Segment Parameters")]
		[SerializeField] TextMeshProUGUI[] fadeTextArray = null;
		[SerializeField] bool lockedSegment = false;
		[SerializeField] float segmentSize = 0f;
		
		
		[Header("Color Parameters")] 
		[SerializeField] Color focusColor = Color.black;
		[SerializeField] Color fadeColor = new Color(0, 0, 0, 0.2f);

		//Internal Methods
		private void Start() {
			InitializeFadeTextColors();
		}

		private void InitializeFadeTextColors() {
			foreach (TextMeshProUGUI text in fadeTextArray) {
				text.color = fadeColor;
			}
		}

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				TriggerFreezeTime();
				StartCoroutine(FadeInSegmentText(other.transform));
			}
		}

		private IEnumerator FadeInSegmentText(Transform player) {
			if (fadeTextArray.Length > 0) {
				float segmentCenter = transform.position.y + (segmentSize / 2);
				while (fadeTextArray[0].color != focusColor) {
					float lerpConstant = Mathf.Abs(segmentCenter - player.position.y) / (segmentSize / 2) ;
					foreach (TextMeshProUGUI text in fadeTextArray) {
						text.color = Color.Lerp(focusColor, fadeColor, lerpConstant);
					}
					yield return null;
				}
			}
		}

		private void OnTriggerExit2D(Collider2D other) {
			if (other.CompareTag("Player") && !lockedSegment) {
				TutorialSegmentSpawner.sharedInstance.SetReadyToSpawn(true);
				StartCoroutine(FadeInSegmentText(other.transform));
			}
		}
		
		private IEnumerator FadeOutSegmentText(Transform player) {
			if (fadeTextArray.Length > 0) {
				float segmentCenter = transform.position.y + (segmentSize / 2);
				while (fadeTextArray[0].color != fadeColor) {
					float lerpConstant = Mathf.Abs(player.position.y - segmentCenter) / (segmentSize / 2) ;
					foreach (TextMeshProUGUI text in fadeTextArray) {
						text.color = Color.Lerp(focusColor, fadeColor, lerpConstant);
					}
					yield return null;
				}
			}
		}

		private void TriggerFreezeTime() {
			TutorialManager tutorialManager = TutorialManager.sharedInstance;
			tutorialManager.FreezeTime(freezeDelay);
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
		public float GetSpawnWavelengthDelay() {
			return spawnWavelengthDelay;
		}
	}
}