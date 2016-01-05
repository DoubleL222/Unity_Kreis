using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingManager{

	private static GameObject segmentPrefab;

	List<GameObject> segments;

	public RingManager (float distance) { //length == distance, 1 unit ~= 1 segment
		if (segmentPrefab == null) {
			segmentPrefab = Resources.Load("_new/Segment") as GameObject;
		}

		int max = (int)distance;
		segments = new List<GameObject> (max);

		float min = -Mathf.PI;
		float step = (2 * Mathf.PI / (float)max);

		for(int i=0; i < max; i++){
			GameObject currentSegment = MonoBehaviour.Instantiate(segmentPrefab/*, new Vector3(min + step * i, distance, 0), new Quaternion()*/) as GameObject;
			SegmentController currentSegmentManager = currentSegment.GetComponent<SegmentController> ();
			currentSegmentManager.SetPosition (new Vector2 ((min + step * i), distance));
			Debug.Log ("Segment position at: " + new Vector2 ((min + step * i), distance));
			segments.Add(currentSegment);
			//Debug.Log ("New segment added");
			//i++;
		}
	}
}
