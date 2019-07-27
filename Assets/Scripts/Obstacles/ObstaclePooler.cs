using System;
using UnityEngine;
using System.Collections.Generic;

public class ObstaclePooler : MonoBehaviour
{
    public static ObstaclePooler sharedInstance;

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
        print("Created Pool For " + obstacle.obstacleID);   //Delete
        obstaclePools.Add(obstacle.obstacleID, new Queue<Obstacle>());
    }

    public void AddToPool(Obstacle obstacle) {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetParent(gameObject.transform);
        try {
            currentQueue = obstaclePools[obstacle.obstacleID];
            currentQueue.Enqueue(obstacle);
        } catch (KeyNotFoundException) {
            CreateEmptyPool(obstacle);
            AddToPool(obstacle);
            return;
        }
        print("Added " + obstacle.obstacleID + " Instance to pool");    //Delete
    }

    public Obstacle GetFromPool(Obstacle obstacle) {
        try {
            currentQueue = obstaclePools[obstacle.obstacleID];
            print("Returned " + obstacle.obstacleID + " Instance from pool");    //Delete
            return currentQueue.Dequeue();
        } catch (InvalidOperationException) {
            print("No Instances for " + obstacle.obstacleID + " left in Pool: Returned Null");    //Delete
            return null;
        } catch (KeyNotFoundException) {
            print("No Entry for " + obstacle.obstacleID + ": Returned Null");    //Delete
            return null;
        }
    }
}
