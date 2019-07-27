using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public int obstacleID = 0;

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Disabler") {
            SendBackToPool();
        } else if (otherCollider.tag == "Player") {
            EndGame(otherCollider.gameObject);
        }
    }

    private void SendBackToPool() {
        ObstaclePooler.sharedInstance.AddToPool(this);
    }

    public GameObject GetInstanceFromPool() {
        try {
            return ObstaclePooler.sharedInstance.GetFromPool(this).gameObject;
        } catch (NullReferenceException) {
            return null;
        }
    }

    private void EndGame(GameObject player) {
        //print("dead");
        //Destroy(player);
        //Player Died, End the Game...
    }
}
