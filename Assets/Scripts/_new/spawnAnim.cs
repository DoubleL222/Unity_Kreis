using UnityEngine;
using System.Collections;

public class spawnAnim : MonoBehaviour {
    public float convergeSpeed = 0.01f;
    public float offsetAmount = 30f;
    public Vector3 center=new Vector3(0,0,0);

    private Vector3 offsetDir;
    public bool atTarget;
    public bool randomDir = true;

    private Vector3 target;
    
    public bool getAtTarget()
    {
        return atTarget;
    }
	// Use this for initialization
	void Start () {
        atTarget = false;
        target = new Vector3();
        offsetDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        if (randomDir)
        {

            offsetDir = Vector3.Normalize(offsetDir);
        }
        else
        {
            offsetDir = Vector3.Normalize(transform.position - center);
        }
        
        transform.position = transform.position + (offsetDir * offsetAmount) + (offsetDir*Random.Range(0,60.0f));
    }
	
	// Update is called once per frame
	void Update () {
        if ((Vector3.Distance(target, transform.localPosition))>0.2f){
            transform.localPosition = transform.localPosition + (target - transform.localPosition) * convergeSpeed;
        }
        else
        {
            transform.localPosition = target;
            atTarget = true;
        }
        
    }
}
