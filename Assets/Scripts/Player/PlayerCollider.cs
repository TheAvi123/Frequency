using System.Collections;

using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
    public class PlayerCollider : MonoBehaviour
    {
        //Reference Variables
        private Transform player = null;
        private new Collider2D collider = null;
        private new Rigidbody2D rigidbody = null;

        //Internal Methods
        void Awake() {
            FindPlayer();
            FindCollider();
            FindRigidbody();
        }

        private void FindPlayer() {
            PlayerWave playerWave = FindObjectOfType<PlayerWave>();
            if (playerWave is null) {
                Debug.LogError("No PlayerWave Found In Scene");
            } else {
                player = playerWave.transform;
            }
        }

        private void FindCollider() {
            collider = GetComponent<Collider2D>();
            if (collider is null) {
                Debug.LogError("No Collider2D Found on PlayerCollider");
            }
        }
        
        private void FindRigidbody() {
            rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody is null) {
                Debug.LogError("No Rigidbody2D Found on PlayerCollider");
            }
        }

        void Update() {
            //if (active) {
            UpdateTransform();
            //}
        }

        private void UpdateTransform() {
            rigidbody.MovePosition(player.position);
            rigidbody.MoveRotation(player.rotation);
        }
        
        // //Helper Methods
        // private void ActivateCollider() {
        //     collider.enabled = true;
        //     active = true;
        // }
        //
        // private void DeactivateCollider() {
        //     active = false;
        //     collider.enabled = false;
        // }
        //
        // private IEnumerator DeactivateAfterDelay(float duration) {
        //     yield return new WaitForSeconds(duration);
        //     DeactivateCollider();
        // }
        //
        // //Public Methods
        // public void ActivateCollider(float duration) {
        //     ActivateCollider();
        //     StartCoroutine(DeactivateAfterDelay(duration));
        // }
    }
}
