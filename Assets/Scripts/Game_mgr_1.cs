using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_mgr_1 : MonoBehaviour {

	// arena
	private GameObject arena;

	public GameObject prefab_player;
	public List<Player> players;

	private Player player;
	public Lane l1, l2, l3;

	public List<Ring> rings;


	// rings
	// players

	public float originalRad = 285.0f;
	float rad1, rad2, rad3;
	public List<Lane> lanes;
	// Use this for initialization
	void Start () {


		lanes = new List<Lane> ();

		float r1 = 0.5f;
		float r2 = 1.0f;
		float r3 = 1.7f;
		float r4 = 2.6f;

		Ring core = new Ring (0,r1);
		core.spawnRing ();
		Ring inner = new Ring (1,r2);
		inner.spawnRing ();
		Ring middle = new Ring (2,r3);
		middle.spawnRing ();
		Ring outer = new Ring (3,r4);
		outer.spawnRing ();

		rings = new List<Ring>();

		rings.Add (core);
		rings.Add (inner);
		rings.Add (middle);
		rings.Add (outer);

		rad1 = (r2 - r1) / 2.0f;
		l1 = new Lane (core, inner,originalRad*r1+originalRad*rad1 );
		rad2 = (r3 - r2) / 2.0f;
		l2 = new Lane (inner, middle, originalRad*r2+originalRad*rad2);
		rad3 = (r4 - r3) / 2.0f;
		l3 = new Lane (middle, outer, originalRad*r3+originalRad*rad3);

		lanes.Add (l1);
		lanes.Add (l2);
		lanes.Add (l3);

		// initializing scripts



		prefab_player = Resources.Load ("Player_prefab_2") as GameObject;

		Debug.Log (prefab_player);

		//script_player = player.GetComponent <Player>();
		//script_player.gameObject.transform
		//Vector3 sdasd = script_player.ORDER_recieve_segment ();

		//player.transform.position = sdasd;

		instantiatePlayer ();


		//Debug.Log("After instantiation");
		//Debug.Log (this.players [0].currentRing);
		//Debug.Log (this.players [0].currentRing.radius_scale_factor);
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("GAME MGR UPDATE");
		//Debug.Log (players[0].currentRing.radius_scale_factor);
		//int id_c, Ring current_ring_c, double mas

		for (int i=0; i<this.players.Count; i++) {

			if(this.players[i].antigrav_changed == false && 
			   this.rings[this.players[i].current_ring_index].get_segment_broken_from_global_offset(this.players[i].offset_arena) == false) continue;

			this.players[i].antigrav_changed = false;

			bool[] brokens = new bool[rings.Count];
			double curr_offset = players[i].offset_arena;

			for(int j=0; j<this.rings.Count; j++) {

				// poizved al so segmenti broken
				brokens[j] = this.rings[j].get_segment_broken_from_global_offset(this.players[i].offset_arena);
			}

			int curr_ring_index = players[i].current_ring_index;

			if(players[i].antigrav == true) {

				curr_ring_index++;

				for(int j=curr_ring_index; j<this.rings.Count; j++) {

					//if(j == this.rings.Count) curr_ring_index = -1;		// fall out

					if(brokens[j] == false) { // found the ring

						break;
					}

					curr_ring_index++;
				}
			}
			else {

				curr_ring_index--;

				for(int j=curr_ring_index; j>=0; j--) {

					if(brokens[j] == false ) {

						break;
					}

					curr_ring_index--;
				}
			}

			// apply

			this.players[i].currentRing = this.rings[curr_ring_index];
			this.players[i].current_ring_index = curr_ring_index;
		}
	}
	
	void instantiatePlayer(){

		//GameObject pf = Instantiate (prefab_player, Vector3.zero, Quaternion.identity) as GameObject;
		//Player pScript = pf.GetComponent<Player> ();
		//pScript.currentRing = l3.outerRing;

		GameObject new_player = Instantiate (prefab_player, Vector3.zero, Quaternion.identity) as GameObject;

		Player pScript = new_player.GetComponent<Player> ();
		//pScript.currentRing = l3.outerRing;
		pScript.currentRing = this.rings [3];
		pScript.current_ring_index = 3;
		pScript.id = 0;

		this.players.Add (pScript);

		GameObject new_player_2 = Instantiate (prefab_player, Vector3.zero, Quaternion.identity) as GameObject;
		
		Player pScript_2 = new_player_2.GetComponent<Player> ();
		//pScript.currentRing = l3.outerRing;
		pScript_2.currentRing = this.rings [2];
		pScript_2.current_ring_index = 2;
		pScript_2.id = 1;

		this.players.Add (pScript_2);

		Debug.Log ("CURR RING");
		Debug.Log (pScript.currentRing.radius_scale_factor * pScript.currentRing.real_radius);
		Debug.Log (pScript.currentRing.id);

		//Debug.Log ("the ring");
		//Debug.Log (l3.outerRing);

		//pScript.currentRing = l3.outerRing;
		//pScript.apply_current_ring (l3);
		//pScript.constructor(0, l3.outerRing, 20);

		//Debug.Log (pScript.currentRing);


	}




	void OnDrawGizmos(){
		if (lanes != null) {
			foreach (Lane l in lanes) {
				Gizmos.DrawWireSphere (Vector3.zero, l.radius);
			}
		}
	}
}