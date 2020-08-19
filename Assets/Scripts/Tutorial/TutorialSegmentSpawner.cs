using Player;

using UnityEngine;

namespace Tutorial
{
	public class TutorialSegmentSpawner : MonoBehaviour
	{
		public static TutorialSegmentSpawner sharedInstance;
		
		//Reference Variables
		private PlayerWave player;
		private Transform playerTransform;
		private Transform objectTransform;
	
		//Configuration Parameters
		[Header("Segments")]
		[SerializeField] private TutorialSegment[] segmentList = null;
		[SerializeField] private float zPosition = 10f;
		
		[Header("Player Position Offsets")]
		[SerializeField] private Vector2 lockedZoneOffset = new Vector2(0.5f, 0.4f);
		[SerializeField] private Vector2 freeZoneOffset = new Vector2(0.5f, 0.25f);
		[SerializeField] private float offsetChangeDuration = 2.5f;
		

		//Player State Variables
		private const float WavelengthDuration = Mathf.PI / 2;
		private float verticalSpeed = 0f;
		private Vector3 oldPosition;

		//Spawner State Variables
		private bool lockedZone = true;
		private bool readyToSpawn = false;
		private int currentSegmentIndex = 0;
		private TutorialSegment currentSegment;
		
		//Internal Methods
		private void Awake() {
			SetSharedInstance();
			VerifySegmentList();
			GetObjectTransform();
			FindPlayer();
		}

		private void SetSharedInstance() {
			sharedInstance = this;
		}

		private void VerifySegmentList() {
			if (segmentList.Length < 1 || segmentList == null) {
				Debug.LogWarning("Tutorial Segment List is Empty or Null");
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
		}
		
		private void GetPlayerMovementParameters() {
			verticalSpeed = player.GetVerticalSpeed();
		}

		private void SetInitialPlayerOffset() {
			ChangeCameraOffset(lockedZoneOffset, 0f);
		}

		private void Update() {
			SpawnTriggerCheck();
		}

		private void SpawnTriggerCheck() {
			if (readyToSpawn) {
				Vector3 newPosition = playerTransform.position;
				if (oldPosition.x < 0 && newPosition.x > 0) {
					if (currentSegmentIndex == segmentList.Length) {
						currentSegmentIndex = 0;
						ChangeCameraOffset(freeZoneOffset, offsetChangeDuration);
					}
					SpawnCurrentSegment();
					currentSegmentIndex++;
				}
				oldPosition = newPosition;
			} 
		}

		private void SpawnCurrentSegment() {
			currentSegment = segmentList[currentSegmentIndex];
			Vector3 spawnPosition = new Vector3(0, playerTransform.position.y, zPosition);
			spawnPosition.y += currentSegment.GetSpawnWavelengthDelay() * WavelengthDuration * verticalSpeed;
			Instantiate(currentSegment, spawnPosition, Quaternion.identity, objectTransform);
			readyToSpawn = false;
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
		public void SetReadyToSpawn(bool status) {
			readyToSpawn = status;
		}
	}
}