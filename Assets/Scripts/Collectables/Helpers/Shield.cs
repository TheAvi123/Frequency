﻿using System.Collections;

using UnityEngine;

using UserInterface;

namespace Collectables.Helpers {
    public class Shield : MonoBehaviour
    {
        //Internal Methods
        private void OnTriggerEnter2D(Collider2D otherCollider) {
            if (otherCollider.CompareTag("ObstaclePart")) {
                DestroyObstacle(otherCollider.gameObject);
            }
        }

        private void DestroyObstacle(GameObject obstacleObject) {
            Transform obstacleTransform = obstacleObject.transform;
            GameObject newObstacle = Instantiate(obstacleObject, obstacleTransform.position, obstacleTransform.rotation);
            obstacleObject.SetActive(false);
            Explodable explodable = newObstacle.GetComponent<Explodable>(); 
            if (explodable) {
                explodable.fragmentInEditor();
                explodable.explode();
            }
            //Spawn Particle Effects
            InfoDisplayer.sharedInstance.DisplayInfo("SHIELD DESTROYED");
            ModifierManager.sharedInstance.EndModifierEffects();
            Destroy(gameObject);
        }

        private IEnumerator ExpireShield() {
            yield return null;
            Destroy(gameObject);
        }

        //Public Methods
        public void DestroyShield() {
            StartCoroutine(ExpireShield());
        }
    }
}
