using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ring_X : MonoBehaviour {

	public float radius = 10;
	public double cushioning = 0.5;
	public double offset_arena = 0.0;

	public GameObject[] segments;

	public GameObject[] players;
	public double[] players_offsets;
	public bool[] player_position_requests;



	/*

	// get local offset, send it to player
	public void ORDER_get_ring_offset_from_global_offset(List<double> message) {

		int plr_id = (int)message[0];
		double plr_offset = message[1];

		if (plr_offset > this.offset_arena) {

			List<double> message_back = new List<double>();

			double value = plr_offset - this.offset_arena;
			message_back.Add(value);

			players[plr_id].SendMessage("ORDER_recieve_relative_offset", message_back);
		}
		else {

			List<double> message_back = new List<double>();

			double missing_to_full = 1.0 - this.offset_arena;
			double value = missing_to_full + plr_offset;
			message_back.Add(value);

			players[plr_id].SendMessage("ORDER_recieve_relative_offset", message_back);
		}
	}


	// get segment, send it to player
	public void ORDER_get_ring_segment_from_global_offset(List<double> message) {
		
		int plr_id = (int)message[0];
		double plr_offset = message[1];
		
		if (plr_offset > this.offset_arena) {

			double value = plr_offset - this.offset_arena;	// local offset
			GameObject wanted_segment = get_segment_from_local_offset(value);

			List<int> message_back = new List<int>();
			message_back.Add(wanted_segment);
			
			players[plr_id].SendMessage("ORDER_recieve_segment", message_back);
		}
		else {

			double missing_to_full = 1.0 - this.offset_arena;
			double value = missing_to_full + plr_offset;	// local offset
			GameObject wanted_segment = get_segment_from_local_offset(value);


			List<int> message_back = new List<int>();
			message_back.Add(wanted_segment);
			
			players[plr_id].SendMessage("ORDER_recieve_segment", message_back);
		}
	}


	// get segment
	public GameObject get_segment_from_local_offset(double offset) {

		int seg_index = this.get_segment_index_from_local_offset (offset);
		return this.segments[seg_index];
	}

	// get segment index
	public int get_segment_index_from_local_offset(double offset) {
		
		return Mathf.Floor (offset / this.segment_delta);
	}


	*/

	// Use this for initialization
	void Start () {
	
		//this.segment_delta = 1.0 / (double)segments.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
