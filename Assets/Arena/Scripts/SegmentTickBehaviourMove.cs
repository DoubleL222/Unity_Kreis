using UnityEngine;
using System.Collections;


/// <summary>
/// Defines behaviour that will move the segment for 'speed' units per second
/// </summary>
public class SegmentTickBehaviourMove : SegmentTickBehaviour{

	Vector3 speed;

	/// <summary>
	/// Initializes a new instance of the <see cref="SegmentTickBehaviourMove"/> class.
	/// </summary>
	/// <param name="speed">Units per second of movement in the x direction in polar coordinates.</param>
	public SegmentTickBehaviourMove(float speed){
		this.speed = new Vector3 (speed, 0, 0);
	}

	void SegmentTickBehaviour.FixedTick(SegmentController segmentController){
		segmentController.physics.transform.Translate (speed * Time.fixedDeltaTime);
	}

	void SegmentTickBehaviour.Tick(SegmentController segmentController){
	}
}
