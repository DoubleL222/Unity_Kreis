using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingManager{

	private static GameObject segmentPrefab;
	public GameObject fatherPrefab;
	List<GameObject> segments;

	public RingManager (float distance) { //length == distance, 1 unit ~= 1 segment
		if (segmentPrefab == null) {
			segmentPrefab = Resources.Load("_new/Segment") as GameObject;
		}
		if (fatherPrefab == null) {
			fatherPrefab = Resources.Load("_new/Father") as GameObject;
		}
		//PREVIOUS
		//int max = distance;
		//END

		int max = (int) Mathf.Floor(distance*2*Mathf.PI);
		segments = new List<GameObject> (max);

		float min = -Mathf.PI;
		float step = (2 * Mathf.PI / (float)max);

		GameObject FatherInstance = MonoBehaviour.Instantiate (fatherPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		for(int i=0; i < max; i++){
			GameObject currentSegment = MonoBehaviour.Instantiate(segmentPrefab/*, new Vector3(min + step * i, distance, 0), new Quaternion()*/) as GameObject;

			currentSegment.transform.parent = FatherInstance.transform;

			SegmentManager currentSegmentManager = currentSegment.GetComponent<SegmentManager> ();
			currentSegmentManager.SetPosition (new Vector2 ((min + step * i), distance));
			Debug.Log ("Segment position at: " + new Vector2 ((min + step * i), distance));
			segments.Add(currentSegment);
			//Debug.Log ("New segment added");
			//i++;
		}
	}
}
