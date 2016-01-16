using UnityEngine;
using System.Collections;

public interface SegmentTickBehaviour{
	void FixedTick (SegmentController segmentController);
	void Tick(SegmentController segmentController);
}
