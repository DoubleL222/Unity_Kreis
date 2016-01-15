using UnityEngine;
using System.Collections;

public class rotateScaleFog : MonoBehaviour {
    public float rotationSpeed = 1;
    public float resizeSpeed = 0.2f;
    public float scaleChange = 0.1f;
    public float sizePhase = 0.0f;
    Vector3 startingScale;
    private float currentRot=0;
    public Vector3 rotationVector;
    public bool randomInit = false;
	// Use this for initialization
	void Start () {
        if (randomInit)
        {
            rotationSpeed = Random.Range(-3,3);
            resizeSpeed = Random.Range(-3, 3);
            sizePhase = Random.Range(0, 120);

        }
        startingScale = transform.localScale;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(rotationVector*rotationSpeed);
        currentRot = currentRot % 360;
        transform.localScale = new Vector3(startingScale.x+(Mathf.Sin((currentRot + sizePhase) *Mathf.Deg2Rad)*scaleChange), startingScale.y + (Mathf.Sin(currentRot * Mathf.Deg2Rad) * scaleChange), startingScale.z);
        currentRot += resizeSpeed;
	}
}
