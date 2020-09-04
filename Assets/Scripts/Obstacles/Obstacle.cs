using System;
using System.Collections.Generic;

using Collectables;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Obstacles {
    public class Obstacle : MonoBehaviour
    {
        //Identity Variables
        [SerializeField] public int obstacleID = 0;
        
        //Reference Variables
        private Transform obstacleTransform;

        //Configuration Parameters
        [SerializeField] private int minCoins = 0, maxCoins = 0;

        //State Variables
        private Coin[] coinPrefabs;
        private TrailRenderer[] coinTrails;
        private List<Coin> coinsSpawned;
        private Coin currentCoin;

        //Internal Methods
        private void Awake() {
            FindCoinPrefabs();
            InitializeSpawnList();
            GetObstacleTransform();
        }

        private void FindCoinPrefabs() {
            coinPrefabs = GetComponentsInChildren<Coin>();
            DisableAllCoins();
        }

        private void InitializeSpawnList() {
            coinsSpawned = new List<Coin>();
        }

        private void GetObstacleTransform() {
            obstacleTransform = gameObject.transform;
        }
        
        private void OnEnable() {
            PrepareObstacle();
        }

        private void PrepareObstacle() {
            ClearCoinTrailRenderers();
            EnableAllObstacleParts();
            DisableAllCoins();
            SpawnModifiers();
            SpawnCoins();
        }
        #region PrepareObstacle Helper Methods
        private void ClearCoinTrailRenderers() {
            coinTrails = GetComponentsInChildren<TrailRenderer>();
            foreach (TrailRenderer ct in coinTrails) {
                ct.Clear();
            }
        }

        private void EnableAllObstacleParts() {
            int childCount = obstacleTransform.childCount;
            for (int i = 0; i < childCount; i++) {
                obstacleTransform.GetChild(i).gameObject.SetActive(true);
            }
        }

        private void DisableAllCoins() {
            foreach (Coin c in coinPrefabs) {
                c.gameObject.SetActive(false);
            }
            InitializeSpawnList();
        }

        private void SpawnModifiers() {
            int childCount = obstacleTransform.childCount;
            for (int i = 0; i < childCount; i++) {
                if (obstacleTransform.GetChild(i).CompareTag("ModifierSpawn")) {
                    ModifierGenerator.sharedInstance.SetAvailableSpawn(obstacleTransform, obstacleTransform.GetChild(i).localPosition);
                }
            }
        }

        private void SpawnCoins() {
            if (minCoins <= coinPrefabs.Length && maxCoins > 0) {
                SpawnXCoins(Random.Range(minCoins, maxCoins + 1));
            } else {
                if (minCoins > coinPrefabs.Length) {
                    Debug.LogError("Minimum Coin Count higher than Coins Prefabs in Obstacle: " + obstacleID);
                }
            }
        }

        private void SpawnXCoins(int x) {
            for (int i = 0; i < x; i++) {
                SpawnRandomCoin();
            }
        }

        private void SpawnRandomCoin() {
            currentCoin = coinPrefabs[Random.Range(0, coinPrefabs.Length)];
            if (coinsSpawned.Contains(currentCoin)) {
                SpawnRandomCoin();
            } else {
                currentCoin.gameObject.SetActive(true);
                currentCoin.gameObject.GetComponent<Animator>().enabled = true;
                coinsSpawned.Add(currentCoin);
            }
        }
        #endregion

        private void Start() {
            ResizeObstacle();
        }

        private void ResizeObstacle() {
            float aspectMultiplier = Camera.main.aspect * 16 / 9;
            obstacleTransform.localScale *= aspectMultiplier;
        }

        //Collision Methods
        private void OnTriggerEnter2D(Collider2D otherCollider) {
            if (otherCollider.CompareTag("Disabler")) {
                SendBackToPool();
            }
        }

        //Public Methods
        public Transform GetInstanceFromPool() {
            try {
                return ObstaclePooler.sharedInstance.GetFromPool(this).transform;
            } catch (NullReferenceException) {
                return null;
            }
        }

        public void SendBackToPool() {
            ObstaclePooler.sharedInstance.AddToPool(this);
        }
    }
}
