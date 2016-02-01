using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputCanvasController : MonoBehaviour {

	public GameObject PlayerLine;
	public RectTransform InputPanel;

	int Lines;
	// Use this for initialization
	void Start () {
		Lines = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			AddNewLine ();
		}
	}
	public void AddNewLine(){
		Lines++;
		GameObject pLine = Instantiate (PlayerLine, Vector3.zero, Quaternion.identity) as GameObject;
		RectTransform pLineT = pLine.GetComponent<RectTransform> ();
		pLineT.SetParent (InputPanel);
		pLineT.anchoredPosition = new Vector3(0.0f, -50*Lines, 0.0f);
	
		}
}
