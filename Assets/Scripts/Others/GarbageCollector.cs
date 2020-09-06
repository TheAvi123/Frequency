using System;

using UnityEngine;

namespace Others
{
	public class GarbageCollector : MonoBehaviour {
		private void OnTriggerExit2D(Collider2D other) {
			if (other.CompareTag("Untagged") || other.CompareTag("Modifier")) {
				Destroy(other.gameObject);
			}
		}
	}
}