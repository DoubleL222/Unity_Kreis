using UnityEngine;
using System.Collections;

public class cameraLoc : MonoBehaviour {
    private GameObject[] players;

    private Vector3 tmpCenter;
    private float tmpSize;
    private float maxDistance = 0;
    private Camera cam;
    private Vector3 zoomCenter;
    private Vector3 startLocation;
    private float startSize;
    

    public float smoothing = 0.05f;
    public float zoomSmoothing = 0.05f;
    public float zoomFactor = 0.4f;

    public static bool updatePlayers = false;
    public static Vector3 center;
    public static float size;

    // Use this for initialization
    void Start () {

        cam = FindObjectOfType<Camera>();
        center = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        tmpCenter = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        startLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        size = cam.orthographicSize;
        tmpSize = cam.orthographicSize;
        startSize = cam.orthographicSize;

        zoomCenter = new Vector3();
	}


    // Update is called once per frame
    void Update() {
        if (updatePlayers)
        {
            print("Camera: updating player array");
            players = GameObject.FindGameObjectsWithTag("PlayerMesh");
            updatePlayers = false;
        }

        if (GameManager.gameEnded)
        {
            tmpCenter = startLocation;

            center = center + (tmpCenter - center) * smoothing;
            //center = new Vector3(center.x, center.y, transform.position.z);

            tmpSize = startSize;
            size = size + (tmpSize - size) * zoomSmoothing;
        }
        else
        {
            if (players != null)
            {
                if (players.Length != 0)
                {
                    foreach (GameObject player in players)
                    {
                        tmpCenter = tmpCenter + player.transform.position;
                        if (Vector3.Distance(player.transform.position, center) > maxDistance)
                        {
                            maxDistance = Vector3.Distance(player.transform.position, zoomCenter);
                        }
                    }
                    tmpCenter = tmpCenter / players.Length;

                    center = center + (tmpCenter - center) * smoothing;
                    center = new Vector3(center.x, center.y, transform.position.z);

                    tmpSize = maxDistance * zoomFactor;
                    size = size + (tmpSize - size) * zoomSmoothing;

                    cam.orthographicSize = size;
                    maxDistance = 0;
                    transform.position = new Vector3(center.x, center.y, transform.position.z);
                    tmpCenter = new Vector3(0, 0, transform.position.z);
                }
            }
        }
        
    }
}
