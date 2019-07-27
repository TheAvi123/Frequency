using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private enum Difficulty {Easy, Medium, Hard, VeryHard}

    //Reference Variables
    private PlayerWave player;

    //Child Reference Variables
    private GameObject easyFolder;
    private GameObject mediumFolder;
    private GameObject hardFolder;
    private GameObject veryHardFolder;

    [Header("Obstacle Prefabs")]
    [SerializeField] private Obstacle[] easyObstacles = null;
    [SerializeField] private Obstacle[] mediumObstacles = null;
    [SerializeField] private Obstacle[] hardObstacles = null;
    [SerializeField] private Obstacle[] veryHardObstacles = null;

    [Header("Obstacle Probabilities")]
    private const float easyProbability = 0.6f;
    private const float mediumProbability = 0.3f;
    private const float hardProbability = 0.09f;
    private const float veryHardProbability = 0.01f;

    [Header("Distances")]
    [SerializeField] private Transform startSpawnPostion = null;
    [SerializeField] float spawnTriggerDistance = 50f;
    [SerializeField] float minObstacleDistance = 5f;
    [SerializeField] float maxObstacleDistance = 12f;

    //Random Variables
    private int currentIndex;           //Random Roll for Obstacle Prefab Selection
    private float currentRoll;          //Random Roll for Obstacle Difficulty Selection
    private float currentDistance;      //Random Spacing between Next Obstacle Spawn

    //State Variables
    private Difficulty currentDifficulty;
    private Obstacle[] currentObstacleList;
    private GameObject currentObstacleFolder;
    private GameObject currentObstacle;
    private Transform lastEndPosition;

    //Methods
    private void Awake() {
        FindPlayerObject();
        SetObstacleFolders();
        PrefabChecks();
    }

    private void FindPlayerObject() {
        player = FindObjectOfType<PlayerWave>();
        if (!player) {
            Debug.LogError("No Player Object Found");
        }
    }

    private void SetObstacleFolders() {
        easyFolder = transform.GetChild(1).gameObject;
        mediumFolder = transform.GetChild(2).gameObject;
        hardFolder = transform.GetChild(3).gameObject;
        veryHardFolder = transform.GetChild(4).gameObject;
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
        }
    }

    private void Start() {
        SpawnFirstObstacle();
    }

    private void SpawnFirstObstacle() {
        currentIndex = Random.Range(0, easyObstacles.Length);        //Always Spawn Easy Obstacle First
        GameObject obstacle = Instantiate(easyObstacles[currentIndex].gameObject, startSpawnPostion) as GameObject;
        obstacle.transform.parent = easyFolder.transform;
        lastEndPosition = obstacle.transform.Find("EndPosition");
    }

    private void Update() {
        SpawnTriggerCheck();
    }

    private void SpawnTriggerCheck() {
        if (lastEndPosition.position.y - player.transform.position.y <= spawnTriggerDistance) {
            ChooseObstacleDifficulty();
            SetObstacleListAndFolder();
            AddRandomSpacing();
            SpawnRandomObstacle();
        }
    }

    private void ChooseObstacleDifficulty() {
        currentRoll = Random.value;
        if (currentRoll < easyProbability) {
            currentDifficulty = Difficulty.Easy;
        } else if (currentRoll < easyProbability + mediumProbability) {
            currentDifficulty = Difficulty.Medium;
        } else if (currentRoll <= easyProbability + mediumProbability + hardProbability) {
            currentDifficulty = Difficulty.Hard;
        } else {
            currentDifficulty = Difficulty.VeryHard;
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
        currentDistance = Random.Range(minObstacleDistance, maxObstacleDistance);
        lastEndPosition.position = new Vector2(lastEndPosition.position.x, (lastEndPosition.position.y + currentDistance));
    }

    private void SpawnRandomObstacle() {
        currentIndex = Random.Range(0, currentObstacleList.Length);
        currentObstacle = currentObstacleList[currentIndex].GetInstanceFromPool();
        if (currentObstacle != null) {
            currentObstacle.SetActive(true);
            currentObstacle.transform.position = lastEndPosition.position;
            SetObstacleProperties(currentObstacle);
        } else {
            currentObstacle = currentObstacleList[currentIndex].gameObject;
            GameObject obstacle = Instantiate(currentObstacle, lastEndPosition) as GameObject;
            SetObstacleProperties(obstacle);
        }
    }

    private void SetObstacleProperties(GameObject obstacle) {
        obstacle.transform.SetParent(currentObstacleFolder.transform);
        lastEndPosition = obstacle.transform.Find("EndPosition");
    }
}
