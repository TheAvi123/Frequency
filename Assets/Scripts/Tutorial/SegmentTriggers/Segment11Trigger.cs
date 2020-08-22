using Player;

using UnityEngine;

namespace Tutorial.SegmentTriggers
{
	public class Segment11Trigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D otherCollider) {
			if (otherCollider.CompareTag("Player")) {
				TutorialManager.sharedInstance.EnableDelayDisplay();
				PlayerAbilityManager player = FindObjectOfType<PlayerAbilityManager>();
				if (player is null) {
					Debug.LogError("No PlayerAbilityManager Found In Scene");
					return;
				}
				player.TriggerCooldowns();
			}
		}
	}
}
