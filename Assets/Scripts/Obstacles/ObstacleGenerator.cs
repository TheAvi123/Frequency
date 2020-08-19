using System.Diagnostics.CodeAnalysis;

using Player;

using Statistics;

using UnityEngine;

namespace Obstacles {
    
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class ObstacleGenerator : MonoBehaviour
    {
        private enum Difficulty {Easy, Medium, Hard, VeryHard}

        //Reference Variables
        private PlayerWave player;
        private Transform playerTransform;
        private ScoreManager scoreManager;

        //Obstacle Parent Folders
        private Transform easyFolder;
        private Transform mediumFolder;
        private Transform hardFolder;
        private Transform veryHardFolder;

        [Header("Obstacle Prefabs")]
        [SerializeField] private Obstacle[] easyObstacles = null;
        [SerializeField] private Obstacle[] mediumObstacles = null;
        [SerializeField] private Obstacle[] hardObstacles = null;
        [SerializeField] private Obstacle[] veryHardObstacles = null;

        [Header("Obstacle Probabilities")]
        const float EasyProbability     = 0.45f;
        const float MediumProbability   = 0.37f;
        const float HardProbability     = 0.15f;
        const float VeryHardProbability = 0.03f;

        [Header("Distances")]
        [SerializeField] Transform startSpawnPostion = null;
        [SerializeField] float spawnTriggerDistance = 50f;
        [SerializeField] float minObstacleDistance = 6f;
        [SerializeField] float maxObstacleDistance = 12f;
        [SerializeField] float distanceReductionRate = 0.02f;

        [Header("Debugging")]
        [SerializeField] Difficulty initialDifficulty = Difficulty.Easy;

        //Random Variables
        private int currentIndex;           //Random Roll for Obstacle Prefab Selection
        private float currentRoll;          //Random Roll for Obstacle Difficulty Selection

        //Difficulty State Variables
        private Difficulty currentMaxDifficulty;
        private Difficulty currentDifficulty;
        private bool reachedMaxDifficulty;

        //Obstacle State Variables
        private Obstacle[] currentObstacleList;
        private Transform currentObstacleFolder;
        private Transform currentObstacle;
        private Vector3 lastEndPosition;

        //Internal Methods
        private void Awake() {
            FindPlayerObject();
            FindScoreManager();
            SetObstacleFolders();
            PrefabChecks();
        }

        private void FindPlayerObject() {
            player = FindObjectOfType<PlayerWave>();
            if (player is null) {
                Debug.LogError("No Player Object Found");
                gameObject.SetActive(false);    //Disable Object
            } else {
                playerTransform = player.transform;
            }
        }

        private void FindScoreManager() {
            scoreManager = ScoreManager.sharedInstance;
            if (!scoreManager) {
                Debug.Log("No Score Manager Found in Scene");
            }
        }

        private void SetObstacleFolders() {
            Transform generator = gameObject.transform;
            easyFolder = generator.GetChild(1);
            mediumFolder = generator.GetChild(2);
            hardFolder = generator.GetChild(3);
            veryHardFolder = generator.GetChild(4);
        }

        private void PrefabChecks() {
            if (easyObstacles == null || easyObstacles.Length == 0) {
                Debug.LogError("No Easy Difficulty Obstacles Set In Inspector");
            }
            if (mediumObstacles == null || mediumObstacles.Length == 0) {
                Debug.LogError("No Medium Difficulty Obstacles Set In Inspector");
            }
            if (hardObstacles == null || hardObstacles.Length == 0) {
                Debug.LogError("No Hard Difficulty Obstacles Set In Inspector");
            }
            if (veryHardObstacles == null || veryHardObstacles.Length == 0) {
                Debug.LogError("No Very Hard Difficulty Obstacles Set In Inspector");
            }
            if (startSpawnPostion == null) {
                Debug.LogError("No Start Spawn Location Assigned To Generator");
                enabled = false;
            }
        }

        private void Start() {
            AspectRatioReconfigurations();
            SetInitialMaxDifficulty();
            SpawnFirstObstacle();
        }

        private void AspectRatioReconfigurations() {
            float aspectMultiplier = Camera.main.aspect * 16 / 9;
            minObstacleDistance *= Mathf.Pow(aspectMultiplier, 2);
            maxObstacleDistance *= Mathf.Pow(aspectMultiplier, 2);
        }

        private void SetInitialMaxDifficulty() {
            currentMaxDifficulty = initialDifficulty;
            reachedMaxDifficulty = false;
        }

        private void SpawnFirstObstacle() {
            currentIndex = Random.Range(0, easyObstacles.Length);       //Always Spawn Easy Obstacle First
            Transform obstacle = Instantiate(easyObstacles[currentIndex].gameObject, startSpawnPostion).transform;
            obstacle.parent = easyFolder;           //Set Parent as Easy Folder for Organization
            lastEndPosition = obstacle.Find("EndPosition").position;
        }

        private void Update() {
            SpawnTriggerCheck();
            UpdateMaxDifficulty();
        }

        private void SpawnTriggerCheck() {
            if (lastEndPosition.y - playerTransform.position.y <= spawnTriggerDistance) {
                ChooseObstacleDifficulty();
                SetObstacleListAndFolder();
                AddRandomSpacing();
                SpawnRandomObstacle();
            }
        }

        private void ChooseObstacleDifficulty() {
            currentRoll = Random.Range(0, GetMaxRollValue());     //Returns Value between 0 and 1
            if (currentRoll < EasyProbability) {
                currentDifficulty = Difficulty.Easy;
            } else if (currentRoll < EasyProbability + MediumProbability) {
                currentDifficulty = Difficulty.Medium;
            } else if (currentRoll <= EasyProbability + MediumProbability + HardProbability) {
                currentDifficulty = Difficulty.Hard;
            } else {
                currentDifficulty = Difficulty.VeryHard;
            }
        }

        private float GetMaxRollValue() {
            switch (currentMaxDifficulty) {
                case Difficulty.Easy:
                    return 0.45f;
                case Difficulty.Medium:
                    return 0.82f;
                case Difficulty.Hard:
                    return 0.97f;
                case Difficulty.VeryHard:
                    return 1.00f;
                default:
                    Debug.LogError("Current Max Difficulty Does Not Fit Enumeration Types");
                    return 1.00f;
            }
        }

        private void SetObstacleListAndFolder() {
            switch (currentDifficulty) {
                case Difficulty.Easy:
                    currentObstacleList = easyObstacles;
                    currentObstacleFolder = easyFolder;
                    break;
                case Difficulty.Medium:
                    currentObstacleList = mediumObstacles;
                    currentObstacleFolder = mediumFolder;
                    break;
                case Difficulty.Hard:
                    currentObstacleList = hardObstacles;
                    currentObstacleFolder = hardFolder;
                    break;
                case Difficulty.VeryHard:
                    currentObstacleList = veryHardObstacles;
                    currentObstacleFolder = veryHardFolder;
                    break;
            }
        }

        private void AddRandomSpacing() {
            lastEndPosition.y += Random.Range(minObstacleDistance, maxObstacleDistance);
        }

        private void SpawnRandomObstacle() {
            currentIndex = Random.Range(0, currentObstacleList.Length);
            currentObstacle = currentObstacleList[currentIndex].GetInstanceFromPool();
            if (currentObstacle) {  //Instance Available in Obstacle Pool
                currentObstacle.position = lastEndPosition;
                currentObstacle.gameObject.SetActive(true);
                SetObstacleProperties(currentObstacle);
            } else {  //No Instance Available in Obstacle Pool
                currentObstacle = currentObstacleList[currentIndex].gameObject.transform;
                Transform obstacle = Instantiate(currentObstacle, lastEndPosition, Quaternion.identity);
                SetObstacleProperties(obstacle);
            }
        }

        private void SetObstacleProperties(Transform obstacle) {
            obstacle.SetParent(currentObstacleFolder);
            lastEndPosition = obstacle.Find("EndPosition").position;
        }

        private void UpdateMaxDifficulty() {
            switch (currentMaxDifficulty) {
                case Difficulty.VeryHard:
                    if (!reachedMaxDifficulty) {
                        ReduceObstacleDistance();
                    }
                    break;
                case Difficulty.Hard:
                    if (scoreManager.GetCurrentScore() >= 100) {
                        currentMaxDifficulty = Difficulty.VeryHard;
                    }
                    break;
                case Difficulty.Medium:
                    if (scoreManager.GetCurrentScore() >= 50) {
                        currentMaxDifficulty = Difficulty.Hard;
                    }
                    break;
                case Difficulty.Easy:
                    if (scoreManager.GetCurrentScore() >= 25) {
                        currentMaxDifficulty = Difficulty.Medium;
                    }
                    break;
                default:
                    Debug.LogError("Current Max Difficulty Does Not Fit Enumeration Types");
                    break;
            }        
        }

        private void ReduceObstacleDistance() {
            if (minObstacleDistance > 0f) {
                minObstacleDistance -= distanceReductionRate * Time.deltaTime;
                maxObstacleDistance -= distanceReductionRate * Time.deltaTime;
            } else if (maxObstacleDistance > 4f) {
                maxObstacleDistance -= distanceReductionRate * Time.deltaTime;
            } else {
                minObstacleDistance = 0f;
                maxObstacleDistance = 4f;
                reachedMaxDifficulty = true;
            }
        
        }
    }
}
