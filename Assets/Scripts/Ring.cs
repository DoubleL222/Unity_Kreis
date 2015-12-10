using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ring {

	public int id = -1;
	public float radius_scale_factor;
	public float real_radius = 285.0f;
	public double cushioning = 26.9; // 69
	public double offset_arena = 0.0;

	private GameObject segPrefab;

	public List<SegmentController> segments;
	private double segment_delta;

	public Ring(int id_c, float r){

		segments = new List<SegmentController> ();
		radius_scale_factor = r;
		segPrefab = Resources.Load ("SegmentPrefab") as GameObject;
		this.cushioning = this.cushioning * r;
		this.id = id_c;
	}
	public void spawnRing(){

		float scale;
		for (int i=0; i<37; i++) {
			GameObject currSegment = MonoBehaviour.Instantiate(segPrefab, Vector3.zero, Quaternion.Euler(0, 0, i*10)) as GameObject;
			currSegment.transform.localScale = new Vector3(radius_scale_factor, radius_scale_factor, radius_scale_factor);



			SegmentController new_seg = currSegment.GetComponentInChildren<SegmentController>();
			this.segments.Add(new_seg);

			Debug.Log("NEW SEGMENT");
			Debug.Log(segments[0]);

			//segments.Add(currSegment.GetComponent<SegmentController>() as SegmentController);
		}
		this.segment_delta = 1.0 / (double)segments.Count;
	}


	// UTIL FUNC ------------------------------------------------------------------------------------
	// ----------------------------------------------------------------------------------------------

	// global offset -> local offset
	public double get_ring_offset_from_global_offset(double offset) {
		
		if (offset > this.offset_arena) {

			double value = offset - this.offset_arena;
			return value;
		}
		else {

			double missing_to_full = 1.0 - this.offset_arena;
			double value = missing_to_full + offset;
			return value;
		}
	}



	// global offset -> Segment.broken
	public bool get_segment_broken_from_global_offset(double offset) {

		SegmentController curr_segment = this.get_ring_segment_from_global_offset (offset);

		Debug.Log ("****************");
		Debug.Log ("****************");
		Debug.Log ("****************");
		Debug.Log ("****************");
		Debug.Log ("CURRENT SEGMENT");

		Debug.Log (curr_segment);

		//if (curr_segment == null)		// FIXIT
			//return false;

		if(curr_segment.destroyed) return true;
		else return false;
	}


	// global offset -> Segment
	public SegmentController get_ring_segment_from_global_offset(double offset) {
		
		if (offset > this.offset_arena) {
			
			double value = offset - this.offset_arena;	// local offset
			SegmentController wanted_segment = get_segment_from_local_offset(value);
			return wanted_segment;
		}
		else {
			
			double missing_to_full = 1.0 - this.offset_arena;
			double value = missing_to_full + offset;	// local offset
			SegmentController wanted_segment = get_segment_from_local_offset(value);
			return wanted_segment;
		}
	}


	// local offset -> segment
	public SegmentController get_segment_from_local_offset(double offset) {
		
		int seg_index = this.get_segment_index_from_local_offset (offset);
		Debug.Log ("SEGMENT INDEX");
		Debug.Log (seg_index);
		return this.segments[seg_index];
	}


	// local offset -> segment index
	public int get_segment_index_from_local_offset(double offset) {

		// clamp
		while (offset > 1.0)
			offset--;
		while (offset < 0.0)
			offset++;

		int index = (int)Mathf.Floor ((float)offset / (float)this.segment_delta);
		if (index >= this.segments.Count)
			index = this.segments.Count - 1;

		return  index;
	}

	// ----------------------------------------------------------------------------------------------
	// END UTIL FUNC --------------------------------------------------------------------------------
}