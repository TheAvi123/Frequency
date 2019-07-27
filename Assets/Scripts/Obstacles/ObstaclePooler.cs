using System;
using UnityEngine;
using System.Collections.Generic;

public class ObstaclePooler : MonoBehaviour
{
    public static ObstaclePooler sharedInstance;    //Shared Static Variable for Other Classes to Access Pool

    [SerializeField] private Dictionary<int, Queue<Obstacle>> obstaclePools;    //Serialized for testing

    //State Variables
    private Queue<Obstacle> currentQueue = null;

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

    ///Pool Management Methods
    private void CreateEmptyPool(Obstacle obstacle) {
        obstaclePools.Add(obstacle.obstacleID, new Queue<Obstacle>());
    }

    public void AddToPool(Obstacle obstacle) {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetParent(gameObject.transform);
        try {
            currentQueue = obstaclePools[obstacle.obstacleID];
            currentQueue.Enqueue(obstacle);     //Add Obstacle to End of Queue
        } catch (KeyNotFoundException) {        //Pool for Obstacle Not Created Yet
            CreateEmptyPool(obstacle);
            AddToPool(obstacle);
            return;
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
