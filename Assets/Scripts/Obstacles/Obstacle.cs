﻿using System;
using System.Collections.Generic;

using Collectables;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Obstacles {
    public class Obstacle : MonoBehaviour
    {
        //Identity Variables
        [SerializeField] public int obstacleID = 0;

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
        }

        private void FindCoinPrefabs() {
            coinPrefabs = GetComponentsInChildren<Coin>();
            DisableAllCoins();
        }

        private void InitializeSpawnList() {
            coinsSpawned = new List<Coin>();
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
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        private void DisableAllCoins() {
            foreach (Coin c in coinPrefabs) {
                c.gameObject.SetActive(false);
            }
            InitializeSpawnList();
        }

        private void SpawnModifiers() {
            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).CompareTag("ModifierSpawn")) {
                    ModifierGenerator.sharedInstance.SetAvailableSpawn(gameObject.transform, transform.GetChild(i).localPosition);
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
            transform.localScale *= aspectMultiplier;
        }

        //Collision Methods
        private void OnTriggerEnter2D(Collider2D otherCollider) {
            if (otherCollider.CompareTag("Disabler")) {
                SendBackToPool();
            }
        }

        //Public Methods
        public GameObject GetInstanceFromPool() {
            try {
                return ObstaclePooler.sharedInstance.GetFromPool(this).gameObject;
            } catch (NullReferenceException) {
                return null;
            }
        }

        public void SendBackToPool() {
            ObstaclePooler.sharedInstance.AddToPool(this);
        }
    }
}
