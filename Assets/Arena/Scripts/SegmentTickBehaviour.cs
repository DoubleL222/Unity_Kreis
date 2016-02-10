using UnityEngine;
using System.Collections;

/// <summary>
/// Interface that will define FixedUpdate behaviour for segments
/// </summary>
public interface SegmentTickBehaviour{
	void FixedTick (SegmentController segmentController);
	void Tick(SegmentController segmentController);
}
