using UnityEngine;
using System.Collections;

public class EmiterDestroyerScript : MonoBehaviour {

	private ParticleSystem[] ps;
	// Use this for initialization
	void Start () {
		ps = GetComponentsInChildren<ParticleSystem> ();
		//ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ps != null)
		{
			bool isAlive = false;
			foreach(ParticleSystem single in ps){
				if(single.IsAlive()){
					isAlive = true;
				}
			}
			if(!isAlive)
			{
			//	Debug.Log("destroying Object");
				Destroy(gameObject);
			}
		}
	}
}
