using System;

using UnityEngine;

namespace Others
{
	public class GarbageCollector : MonoBehaviour {
		private void OnTriggerExit2D(Collider2D other) {
			if (other.CompareTag("Untagged") || other.CompareTag("Modifier")) {
				Debug.Log("Destroyed " + other.gameObject.name + " w/ Tag " + other.gameObject.tag);
				Destroy(other.gameObject);
			} else {
				Debug.Log("Did Not Destroy " + other.gameObject.name + " w/ Tag " + other.gameObject.tag);
			}
		}
	}
}