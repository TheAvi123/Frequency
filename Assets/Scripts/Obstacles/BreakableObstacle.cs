using System;

using UnityEngine;

namespace Obstacles 
{
    [RequireComponent(typeof(Explodable))]
    public class BreakableObstacle : MonoBehaviour {

        //Configuration Parameters
        [SerializeField] private int explosionPoints = 0;

        //Internal Methods
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("PlayerCollider")) {
                Transform obstacleTransform = gameObject.transform;
                GameObject newObstacle = Instantiate(gameObject, obstacleTransform.position, obstacleTransform.rotation);
                gameObject.SetActive(false);
                Explodable explodable = newObstacle.GetComponent<Explodable>(); 
                if (explodable) {
                    explodable.extraPoints = explosionPoints;
                    explodable.fragmentInEditor();
                    explodable.explode();
                }
            }
        }
    }
}
