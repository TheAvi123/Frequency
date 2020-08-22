using System.Collections;

using Player;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
	public class TutorialSegment : MonoBehaviour
	{
		//Reference Variables
		private TutorialSegmentSpawner segmentSpawner;

		//Configuration Parameters
		[Header("Spawn Variables")]
		[SerializeField] float spawnWavelengthDelay = 1f;
		[SerializeField] float entryAngle = 0f;
		
		[Header("Segment Parameters")]
		[SerializeField] TextMeshProUGUI[] fadeTextArray = null;
		[SerializeField] Image[] fadeImageArray = null;
		[SerializeField] float segmentSize = 0f;

		[Header("Color Parameters")] 
		[SerializeField] Color focusColor = Color.black;
		[SerializeField] Color fadeColor = new Color(0, 0, 0, 0.2f);

		//Internal Methods
		private void Awake() {
			FindSegmentSpawner();
		}

		private void FindSegmentSpawner() {
			segmentSpawner = FindObjectOfType<TutorialSegmentSpawner>();
			if (segmentSpawner is null) {
				Debug.LogError("No Segment Spawner Found In Tutorial");
			}
		}

		private void Start() {
			InitializeFadeTextColors();
			ResizeSegment();
		}

		private void InitializeFadeTextColors() {
			foreach (TextMeshProUGUI text in fadeTextArray) {
				if (text is null) {
					Debug.Log(gameObject.name);
				}
				text.color = fadeColor;
			}
			foreach (Image image in fadeImageArray) {
				if (image is null) {
					Debug.Log(gameObject.name);
				}
				image.color = Color.clear;
			}
		}

		private void ResizeSegment() {
			float aspectMultiplier = Camera.main.aspect * 16 / 9;
			transform.localScale *= aspectMultiplier;
		}

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				other.GetComponent<PlayerWave>().SetIdealAngle(entryAngle);
				segmentSpawner.SpawnNextSegment();
				StartCoroutine(FadeSegmentText(other.transform));
				StartCoroutine(FadeSegmentImages(other.transform));
			}
		}

		private void OnTriggerExit2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				StartCoroutine(FadeSegmentText(other.transform));
				StartCoroutine(FadeSegmentImages(other.transform));
			}
		}

		private IEnumerator FadeSegmentText(Transform player) {
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
		
		private IEnumerator FadeSegmentImages(Transform player) {
			if (fadeImageArray.Length > 0) {
				float segmentCenter = transform.position.y + (segmentSize / 2);
				while (fadeImageArray[0].color != Color.black) {
					float lerpConstant = Mathf.Abs(segmentCenter - player.position.y) / (segmentSize / 2) ;
					foreach (Image image in fadeImageArray) {
						image.color = Color.Lerp(Color.black, Color.clear, lerpConstant);
					}
					yield return null;
				}
			}
		}

		//Public Methods
		public float GetSpawnWavelengthDelay() {
			return spawnWavelengthDelay;
		}
	}
}