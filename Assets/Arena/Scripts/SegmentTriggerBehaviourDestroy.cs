using UnityEngine;
using System.Collections;

public class SegmentTriggerBehaviourDestroy : SegmentTriggerBehaviour {
	void SegmentTriggerBehaviour.Enter (Collider2D other, SegmentController segmentController){
		if (other.gameObject.tag == "Shot") {
			Debug.Log ("Segment triggered with shot");
			ShotDestroyerScript SDS = other.GetComponent<ShotDestroyerScript> ();
			if (SDS.IsUsed == false) {
				Vector3 spawnPos = segmentController.transformToPolar (other.transform.position);
				segmentController.DestroySegment (spawnPos);
				SDS.IsUsed = true;
			}
		}

	}
}
