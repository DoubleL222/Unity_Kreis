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
    public bool stationary = true;
    public float MaxDist = 0.1f;
    public float MaxZoom = 0.1f;

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

        zoomCenter = new Vector3(0,0,0);
	}


    // Update is called once per frame
    void Update() {
        if (updatePlayers)  //ko se unici igralec, zacne igra se prestavi update players na true
        {
            //print("Camera: updating player array");
            players = GameObject.FindGameObjectsWithTag("PlayerMesh");
            updatePlayers = false;
        }

        if (GameManager.gameEnded)//vrni kamero na izvorno lokacijo
        {
            tmpCenter = startLocation;

            center = center + (tmpCenter - center) * smoothing;
            //center = new Vector3(center.x, center.y, transform.position.z);

            tmpSize = startSize;
            size = size + (tmpSize - size) * zoomSmoothing;
        }
        else
        {
            if (players != null)//cakaj na igralce
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

                    if (stationary)
                    {

                        tmpCenter = tmpCenter / players.Length;//izracun lokacije kamere
                        if (Vector3.Distance(tmpCenter,zoomCenter) > MaxDist)
                        {
                            tmpCenter = (tmpCenter - zoomCenter) * MaxDist;
                        }

                        tmpSize = maxDistance * zoomFactor;//velikost je odvisna od razdalje najbolj oddaljenega igralca
                        if (tmpSize < MaxZoom)
                        {
                            tmpSize = MaxZoom;
                        }
                    }
                    else
                    {
                        tmpCenter = tmpCenter / players.Length;//izracun lokacije kamere
                        tmpSize = maxDistance * zoomFactor;//velikost je odvisna od razdalje najbolj oddaljenega igralca
                    }
                    

                    center = center + (tmpCenter - center) * smoothing;
                    center = new Vector3(center.x, center.y, transform.position.z);
                    
                    size = size + (tmpSize - size) * zoomSmoothing;//izracun velikosti

                    cam.orthographicSize = size;
                    maxDistance = 0;
                    transform.position = new Vector3(center.x, center.y, transform.position.z);
                    tmpCenter = new Vector3(0, 0, transform.position.z);
                }
            }
        }
        
    }
}
