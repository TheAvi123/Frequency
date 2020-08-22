using Player;

using UnityEngine;

namespace Tutorial.SegmentTriggers
{
	public class SegmentEndTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D otherCollider) {
			if (otherCollider.CompareTag("Player")) {
				TutorialManager.sharedInstance.EndTutorial();
			}
		}
	}
}
