using UnityEngine;
using System.Collections;

public class imageSeq : MonoBehaviour {
    int iterator = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Application.CaptureScreenshot("screens\\img"+iterator+".png",1);
        iterator++;
    }
}
