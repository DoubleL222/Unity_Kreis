using UnityEngine;
using System.Collections;

public class SegmentTickBehaviourMove : SegmentTickBehaviour{

	float speed;

	public SegmentTickBehaviourMove(float speed){
		this.speed = speed;
	}

	void SegmentTickBehaviour.FixedTick(SegmentController segmentController){
		Vector3 pos = segmentController.physics.transform.position;
		pos.x += speed * Time.fixedDeltaTime;
		segmentController.physics.transform.position = pos;
	}

	void SegmentTickBehaviour.Tick(SegmentController segmentController){
	}
}
