using Player;

using UnityEngine;

namespace Tutorial
{
	public class TutorialSegmentSpawner : MonoBehaviour
	{
		//Reference Variables
		private PlayerWave player;
		private Transform playerTransform;
		private Transform objectTransform;
	
		//Configuration Parameters
		[SerializeField] private float zPosition = 10f;
		[SerializeField] private TutorialSegment[] segmentList = null;
		[SerializeField] private Vector2 tutorialCameraOffset = new Vector2(0.5f, 0.45f);

		//Player State Variables
		private const float WavelengthDuration = Mathf.PI / 2;
		private float verticalSpeed = 0f;
		private Vector3 oldPosition;

		//Spawner State Variables
		private TutorialSegment currentSegment;
		private int currentSegmentIndex = 0;
		
		//Internal Methods
		private void Awake() {
			VerifySegmentList();
			GetObjectTransform();
			FindPlayer();
		}

		private void VerifySegmentList() {
			if (segmentList.Length == 0 || segmentList == null) {
				Debug.LogError("Tutorial Segment List is Empty or Null");
				enabled = false;
			}
		}

		private void GetObjectTransform() {
			objectTransform = gameObject.transform;
		}

		private void FindPlayer() {
			player = FindObjectOfType<PlayerWave>();
			if (player is null) {
				Debug.LogError("No Player Found In Tutorial Scene");
				enabled = false;
			} else {
				playerTransform = player.transform;
			}
		}

		private void Start() {
			GetPlayerMovementParameters();
			SetInitialPlayerOffset();
			SpawnFirstSegment();
		}

		private void GetPlayerMovementParameters() {
			verticalSpeed = player.GetVerticalSpeed();
		}

		private void SetInitialPlayerOffset() {
			ChangeCameraOffset(tutorialCameraOffset, 0f);
		}

		private void SpawnFirstSegment() {
			currentSegmentIndex = 0;
			SpawnCurrentSegment();
		}

		//Helper Methods
		private void SpawnCurrentSegment() {
			currentSegment = segmentList[currentSegmentIndex];
			Vector3 spawnPosition = new Vector3(0, playerTransform.position.y, zPosition);
			spawnPosition.y += currentSegment.GetSpawnWavelengthDelay() * WavelengthDuration * verticalSpeed;
			Instantiate(currentSegment, spawnPosition, Quaternion.identity, objectTransform);
		}
		
		private void ChangeCameraOffset(Vector2 offsetCoordinates, float changeDuration) {
			PlayerFollow camFollow = Camera.main.GetComponent<PlayerFollow>();
			if (camFollow) {
				camFollow.ChangeCameraOffset(offsetCoordinates, changeDuration);
			} else {
				Debug.LogWarning("No PlayerFollow Script Found On Main Camera In Tutorial");
			}
		}

		//Public Methods
		public void SpawnNextSegment() {
			currentSegmentIndex++;
			SpawnCurrentSegment();
		}
	}
}