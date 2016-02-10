using UnityEngine;
using System.Collections;

/// <summary>
/// Defines behavioru that will destroy the segment when it is hit by a bullet
/// </summary>

public class SegmentTriggerBehaviourDestroy : SegmentTriggerBehaviour {
	
	void SegmentTriggerBehaviour.Enter (Collider2D other, SegmentController segmentController){		
		
		if (other.gameObject.tag == "Shot") {
			ShotDestroyerScript SDS = other.GetComponent<ShotDestroyerScript> ();
			if (SDS.IsUsed == false) {	//if the shot has not yet hit something, destroy the segment and mark the shot used
				Vector3 spawnPos = UtilityScript.transformToCartesian (other.transform.position);
				segmentController.DestroySegment (spawnPos);
				SDS.IsUsed = true;
			}
		}

	}
}
