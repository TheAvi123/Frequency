using System;

using UnityEngine;

namespace Tutorial
{
	public class TutorialSegmentSpawner : MonoBehaviour
	{
		//Reference Variables
	
		//Configuration Parameters
		[SerializeField] private TutorialSegment[] segmentList;

		//State Variables
		private void Awake() {
			VerifySegmentList();
		}

		private void VerifySegmentList() {
			if (segmentList.Length < 1 || segmentList == null) {
				Debug.LogError("Tutorial Segment List is Empty or Null");
			}
		}

		private void Update() {
			throw new NotImplementedException();
		}
	}
}