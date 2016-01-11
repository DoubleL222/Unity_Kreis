using UnityEngine;
using System.Collections;

public interface SegmentCollisionBehaviour{
	void Enter (Collision2D col, SegmentController segmentController);
}
