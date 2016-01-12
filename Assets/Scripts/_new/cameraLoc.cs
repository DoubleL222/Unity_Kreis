using UnityEngine;
using System.Collections;

public class cameraLoc : MonoBehaviour {
    GameObject[] players;
    Vector3 center;
    Vector3 tmpCenter;
    float maxDistance = 0;
    Camera cam;
    public float smoothing = 0.05f;
    public float zoomFactor = 0.4f;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        cam = FindObjectOfType<Camera>();
        center = new Vector3(0, 0, transform.position.z);
        tmpCenter=new Vector3(0, 0, transform.position.z);

        foreach( GameObject player in players){
            tmpCenter += player.transform.position;
            if (Vector3.Distance(player.transform.position, center) > maxDistance)
            {
                maxDistance = Vector3.Distance(player.transform.position, center);
            }
        }
        tmpCenter /= players.Length;

        center = center + (tmpCenter - tmpCenter) * smoothing;
        center.z = transform.position.z;
        transform.position = center;
	}
	
	// Update is called once per frame
	void Update () {
        
        foreach (GameObject player in players)
        {
            tmpCenter = tmpCenter + player.transform.position;
            if (Vector3.Distance(player.transform.position, center) > maxDistance)
            {
                maxDistance = Vector3.Distance(player.transform.position, center);
            }
        }
        tmpCenter = tmpCenter/players.Length;

        center = center + (tmpCenter - center) * smoothing;
        cam.orthographicSize = Mathf.Pow(maxDistance/4,2) * zoomFactor;
        maxDistance = 0;
        transform.position = new Vector3(center.x, center.y, transform.position.z);
        tmpCenter = new Vector3(0, 0, transform.position.z);
	}
}
