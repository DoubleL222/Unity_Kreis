using UnityEngine;
using System.Collections;

/// <summary>
/// Interface that will define trigger behaviour for segments.
/// </summary>
public interface SegmentTriggerBehaviour{
	void Enter (Collider2D other, SegmentController segmentController);
}
