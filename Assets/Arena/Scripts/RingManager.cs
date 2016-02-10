using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class that represents a ring of segments
/// </summary>
public class RingManager{
	private static GameObject segmentPrefab;
	List<GameObject> segments;
	public List<SegmentController> segmentControlers;	

	public RingManager (float distance, List<SegmentCollisionBehaviour> segmentCollisionBehaviours, List<SegmentTickBehaviour> segmentTickBehaviours, List<SegmentTriggerBehaviour> segmentTriggerBehaviours, Sprite sprite) {
		if (segmentPrefab == null) {
			segmentPrefab = Resources.Load("Segment") as GameObject;
		}
		segmentControlers = new List<SegmentController> ();

		int max = (int)Mathf.Floor (distance * Mathf.PI * 1.25f); // number of segments

		segments = new List<GameObject> (max);

		float min = -Mathf.PI;						//First segment position
		float step = (2 * Mathf.PI / (float)max);	//Distance between segments
	
		//SegmentTickBehaviourMove stbm = new SegmentTickBehaviourMove (tickMove); //Segments with this tick behaviour will move
		//SegmentTriggerBehaviourDestroy stbd = new SegmentTriggerBehaviourDestroy (); //Segments with this tick behaviour will be destroyed upon getting shot

		GameObject SegmentsParent = new GameObject ();
		SegmentsParent.name = "SegmentsParent"+max;
		SegmentsParent.transform.position = Vector3.zero;
		SegmentsParent.transform.rotation = Quaternion.identity;

		for(int i=0; i < max; i++){
			GameObject currentSegment = MonoBehaviour.Instantiate(segmentPrefab) as GameObject;
			SegmentController currentSegmentManager = currentSegment.GetComponentInChildren<SegmentController> ();
			segmentControlers.Add (currentSegmentManager);
			segments.Add(currentSegment);

			currentSegmentManager.SetPosition (new Vector2 ((min + step * i + 0.0001f), distance)); 

			currentSegmentManager.mesh.transform.Find ("barrier").GetComponent<SpriteRenderer> ().sprite = sprite;


			for (int j = 0; j < segmentCollisionBehaviours.Count; j++)
				currentSegmentManager.addBehaviour (segmentCollisionBehaviours [j]);
			for (int j = 0; j < segmentTriggerBehaviours.Count; j++)
				currentSegmentManager.addBehaviour(segmentTriggerBehaviours[j]);
			for (int j = 0; j < segmentTickBehaviours.Count; j++)
				currentSegmentManager.addBehaviour(segmentTickBehaviours[j]);

			currentSegment.transform.SetParent (SegmentsParent.transform);
		}
	}
}
