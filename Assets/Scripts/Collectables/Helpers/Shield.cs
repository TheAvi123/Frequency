using System.Collections;
using System.Security.Cryptography;

using Player;

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
                Vector2 scale = obstacleObject.transform.localScale;
                explodable.extraPoints = (int) (explodable.extraPoints * scale.x * scale.y);
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
