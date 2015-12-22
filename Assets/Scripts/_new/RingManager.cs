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

		float min = -distance;
		float step = (distance / max);

		for(int i=0; i < max*2; i++){
			GameObject currentSegment = MonoBehaviour.Instantiate(segmentPrefab/*, new Vector3(min + step * i, distance, 0), new Quaternion()*/) as GameObject;
			SegmentManager currentSegmentManager = currentSegment.GetComponent<SegmentManager> ();
			currentSegmentManager.SetPosition (new Vector2 ((min + step * i), distance));
			segments.Add(currentSegment);
			//Debug.Log ("New segment added");
		}
	}
}
