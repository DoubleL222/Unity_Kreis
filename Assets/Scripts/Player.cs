using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public int id = 0;
	private bool dead = false;

	public bool antigrav_changed = false;

	public Ring currentRing;
	public int current_ring_index;

	public bool antigrav = true;
	private double mass = 20.0;

	private double velocity_offset = 0.0;
	private double velocity_offset_max = 0.01;

	private double velocity_height = 0.0;
	private double velocity_height_max = 20.0;

	public double offset_arena = 0.0;
	public double height_arena = 9.5;

	private double offset_ring = 0.0;
	private double height_ring = 0.0;

	public float shotSpeed = 500.0f;
	public float fireRate = 2.0f;
	GameObject shotPrefab;
	float lastFire = 0.0f;
	// Use this for initialization
	void Start () {
		shotPrefab = Resources.Load ("Shot_Prefab") as GameObject;

	}

	public Player constructor (int id_c, Ring current_ring_c, double mass) {

		this.id = id_c;
		this.currentRing = current_ring_c;
		this.mass = mass;

		return this;
	}

	public void apply_current_ring(Ring new_ring) {

		this.currentRing = new_ring;
	}

	public void fire(){

		int inverse = antigrav ? 1 : -1;
		Vector3 fireDir = Vector3.zero - transform.position*inverse;
		Vector3 shotOffset = fireDir.normalized * 100.0f;
		GameObject shotInstance = Instantiate (shotPrefab, transform.position + shotOffset, Quaternion.identity) as GameObject;
		shotInstance.GetComponent<Rigidbody> ().velocity = fireDir.normalized * 300;
		Debug.Log (fireDir.normalized);
	}

	public void update_position() {

		Debug.Log("METHOD : Update position");
		//Debug.Log (this);

		this.offset_arena += this.velocity_offset;
		this.offset_ring += this.velocity_offset;

		// clamping

		if (this.offset_arena < 0.0) {
			while(this.offset_arena < 0.0) this.offset_arena += 1;
		}
	
		if (this.offset_arena > 1.0) {
			while(this.offset_arena > 1.0) this.offset_arena -= 1;
		}
			
		if (this.offset_ring < 0.0) {
			while(this.offset_ring < 0.0) this.offset_ring += 1;
		}
	
		if (this.offset_ring > 1.0){
			while(this.offset_ring > 1.0) this.offset_ring -= 1;
		}


		Debug.Log (this.offset_arena);
		Debug.Log (this.offset_ring);

		// current ring

		this.transform.position = new Vector3(0,0,0);
		this.transform.rotation = Quaternion.identity;
		//Quaternion deg_90 = Quaternion.AngleAxis (90, new Vector3(0,0,1));
		//this.transform.rotation = deg_90 * this.transform.rotation;

		//Debug.Log(" CURR RING -----------");
		//Debug.Log (currentRing);

		double real_radius = currentRing.real_radius * currentRing.radius_scale_factor;

		Debug.Log ("Real radius" + real_radius);

		if (this.antigrav == false) {

			Quaternion qt_180 = Quaternion.AngleAxis (180, new Vector3 (0, 0, 1));
			this.transform.rotation = qt_180 * this.transform.rotation;


			double pos_offset = real_radius + currentRing.cushioning + velocity_height;
			Vector3 set_vec = new Vector3(0,(float)-pos_offset, 0);
			this.transform.position = set_vec;

			//Quaternion qt_course = Quaternion.AngleAxis((int)(360 * this.offset_arena), new Vector3(0,0,1));
			//this.transform.rotation = qt_course * this.transform.rotation;
		} else {

			Debug.Log("ANTIGRAAAV");

			double pos_offset = real_radius - currentRing.cushioning - velocity_height;
			Vector3 set_vec = new Vector3(0,(float)-pos_offset, 0);
			this.transform.position = set_vec;

			//Quaternion qt_course = Quaternion.AngleAxis((int)(360 * this.offset_arena), new Vector3(0,0,1));
			//this.transform.rotation = qt_course * this.transform.rotation;
		}

		this.transform.RotateAround(Vector3.zero, new Vector3(0,0,1), (float)this.offset_arena*360f);
	}


	public int timer_gravity_max = 50;
	public int timer_gravity = 50;
	
	// Update is called once per frame
	void Update () {
	
		Debug.Log ("**************");
		Debug.Log ("PLAYER UPDATE");

		Debug.Log (this.id);

		Debug.Log (this.currentRing);

		if (this.id == 0) {

			// KEY HANDLER
			if (Input.GetKey (KeyCode.D)) {
				if (lastFire + fireRate < Time.time) {
					fire ();
					lastFire = Time.time;
				}

			}
			if (Input.GetKey ("q")) {	// left

				//Debug.Log("KEY : A");

				if (this.antigrav)
					this.velocity_offset = this.velocity_offset_max;
				else
					this.velocity_offset = -this.velocity_offset_max;
			} else if (Input.GetKey ("w")) {	// right

				//Debug.Log("KEY : D");

				if (this.antigrav)
					this.velocity_offset = -this.velocity_offset_max;
				else
					this.velocity_offset = this.velocity_offset_max;
			} else {

				this.velocity_offset = 0;
			}


			if (Input.GetKey ("a")) {  // jump

				this.velocity_height = this.velocity_height_max;
			} else {

				this.velocity_height = 0.0;
			}

			if (Input.GetKey ("s")) {

				this.timer_gravity --;
				if (this.timer_gravity <= 0) {

					if (this.antigrav == true)
						this.antigrav = false;
					else
						this.antigrav = true;

					this.timer_gravity = this.timer_gravity_max;
					this.antigrav_changed = true;
				}

				//this.antigrav_changed = true;
			}

		} else {


			// KEY HANDLER
			if (Input.GetKey (KeyCode.L)) {
				if (lastFire + fireRate < Time.time) {
					fire ();
					lastFire = Time.time;
				}
				
			}
			if (Input.GetKey ("u")) {	// left
				
				//Debug.Log("KEY : A");
				
				if (this.antigrav)
					this.velocity_offset = this.velocity_offset_max;
				else
					this.velocity_offset = -this.velocity_offset_max;
			} else if (Input.GetKey ("i")) {	// right
				
				//Debug.Log("KEY : D");
				
				if (this.antigrav)
					this.velocity_offset = -this.velocity_offset_max;
				else
					this.velocity_offset = this.velocity_offset_max;
			} else {
				
				this.velocity_offset = 0;
			}
			
			
			if (Input.GetKey ("j")) {  // jump

				this.velocity_height = this.velocity_height_max;
			} else {
				
				this.velocity_height = 0.0;
			}
			
			if (Input.GetKey ("k")) {
				
				this.timer_gravity --;
				if (this.timer_gravity <= 0) {
					
					if (this.antigrav == true)
						this.antigrav = false;
					else
						this.antigrav = true;
					
					this.timer_gravity = this.timer_gravity_max;
					this.antigrav_changed = true;
				}
				
				//this.antigrav_changed = true;
			}
		}

		this.update_position ();
	}
	void OnCollisionEnter(Collision col){
		Debug.Log ("Coll entered");
		if (col.gameObject.tag == "Shot") {
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}
}

