using UnityEngine;
using System.Collections;

public interface SegmentTriggerBehaviour{
	void Enter (Collider2D other, SegmentController segmentController);
}
