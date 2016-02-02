using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingManager{

	private static GameObject segmentPrefab;

	List<GameObject> segments;

	public List<SegmentController> segmentControlers;	

	public RingManager (float distance, float tickMove) {
		if (segmentPrefab == null) {
			segmentPrefab = Resources.Load("Segment") as GameObject;
		}
		segmentControlers = new List<SegmentController> ();

		int max = (int)Mathf.Floor (distance * Mathf.PI * 1.25f);

		segments = new List<GameObject> (max);

		float min = -Mathf.PI;
		float step = (2 * Mathf.PI / (float)max);
		//Debug.Log ("# segments in ring " + max);
	
		SegmentTickBehaviourMove stbm = new SegmentTickBehaviourMove (tickMove);
		SegmentTriggerBehaviourDestroy stbd = new SegmentTriggerBehaviourDestroy ();

		GameObject SegmentsParent = new GameObject ();
		SegmentsParent.name = "SegmentsParent"+max;
		SegmentsParent.transform.position = Vector3.zero;
		SegmentsParent.transform.rotation = Quaternion.identity;
		for(int i=0; i < max; i++){
			GameObject currentSegment = MonoBehaviour.Instantiate(segmentPrefab/*, new Vector3(min + step * i, distance, 0), new Quaternion()*/) as GameObject;
			SegmentController currentSegmentManager = currentSegment.GetComponentInChildren<SegmentController> ();
			segmentControlers.Add (currentSegmentManager);
			currentSegmentManager.SetPosition (new Vector2 ((min + step * i + 0.0001f), distance));
			currentSegmentManager.addBehaviour (stbm);
			currentSegmentManager.addBehaviour (stbd);
			//currentSegmentManager.SetPosition (new Vector2 ((min + step * i), distance));
			segments.Add(currentSegment);
			currentSegment.transform.SetParent (SegmentsParent.transform);
		}
	}
}
