using UnityEngine;
using System.Collections;

public class ButtonRightGetter : MonoBehaviour
{
	public InputHandler inputHandler;

	// Use this for initialization
	void OnMouseDown()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.rightPressed = true;
		inputHandler.inputChanged = true;
	}
	
	void OnMouseUp()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.rightPressed = false;
		inputHandler.inputChanged = true;
	}

}