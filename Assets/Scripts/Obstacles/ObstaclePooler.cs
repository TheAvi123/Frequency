using System;
using System.Collections.Generic;

using UnityEngine;

namespace Obstacles {
    public class ObstaclePooler : MonoBehaviour
    {
        public static ObstaclePooler sharedInstance;

        //Collections
        private Dictionary<int, Queue<Obstacle>> obstaclePools;

        //State Variables
        private Queue<Obstacle> currentQueue = null;

        //Internal Methods
        private void Awake() {
            SetObstaclePoolerInstance();
            InstantiateDictionary();
        }

        private void SetObstaclePoolerInstance() {
            sharedInstance = this;
        }

        private void InstantiateDictionary() {
            obstaclePools = new Dictionary<int, Queue<Obstacle>>();
        }

        //Public Methods
        public void CreateEmptyPool(Obstacle obstacle) {
            obstaclePools.Add(obstacle.obstacleID, new Queue<Obstacle>());
        }

        public void AddToPool(Obstacle obstacle) {
            obstacle.gameObject.SetActive(false);
            obstacle.transform.SetParent(gameObject.transform);
            try {
                currentQueue = obstaclePools[obstacle.obstacleID];
                currentQueue.Enqueue(obstacle);
            } catch (KeyNotFoundException) {        //Pool for Obstacle Not Created Yet
                CreateEmptyPool(obstacle);
                AddToPool(obstacle);
            }
        }

        public Obstacle GetFromPool(Obstacle obstacle) {
            try {
                currentQueue = obstaclePools[obstacle.obstacleID];
                return currentQueue.Dequeue();
            } catch (InvalidOperationException) {   //Pool Created but No Instances Available
                return null;
            } catch (KeyNotFoundException) {        //Pool for Obstacle Not Created Yet
                return null;
            }
        }
    }
}
