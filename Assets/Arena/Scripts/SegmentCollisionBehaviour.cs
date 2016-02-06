using UnityEngine;
using System.Collections;
/// <summary>
/// Interface that will define collisio behaviour for segments
/// </summary>
public interface SegmentCollisionBehaviour{
	void Enter (Collision2D col, SegmentController segmentController);
}
