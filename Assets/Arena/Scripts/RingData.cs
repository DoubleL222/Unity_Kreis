using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingData{
	public RingData(){
		segmentCollisionBehaviours = new List<SegmentCollisionBehaviour> ();
		segmentTickBehaviours = new List<SegmentTickBehaviour> ();
		segmentTriggerBehaviours = new List<SegmentTriggerBehaviour> ();
	}
	public Sprite sprite;
	public float size;
	public List<SegmentCollisionBehaviour> segmentCollisionBehaviours;
	public List<SegmentTickBehaviour> segmentTickBehaviours;
	public List<SegmentTriggerBehaviour> segmentTriggerBehaviours;
}
