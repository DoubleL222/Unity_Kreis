using UnityEngine;
using System.Collections;

public class SegmentTriggerBehaviourDestroy : SegmentTriggerBehaviour {
	void SegmentTriggerBehaviour.Enter (Collider2D other, SegmentController segmentController){
		if (other.CompareTag ("Shot"))
			Debug.Log ("Assplode!");
	}
}
