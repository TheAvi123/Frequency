using UnityEngine;

namespace Obstacles 
{ 
    public class FunObstacle : MonoBehaviour {

        //Internal Methods
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("PlayerCollider")) {
                Explodable explodable = GetComponent<Explodable>(); 
                if (explodable) {
                    explodable.fragmentInEditor();
                    explodable.explode();
                }
            }
        }
    }
}
