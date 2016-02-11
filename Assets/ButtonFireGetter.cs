using UnityEngine;
using System.Collections;

public class ButtonFireGetter : MonoBehaviour
{
	public InputHandler inputHandler;

	void OnMouseDown()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.firePressed = true;
		inputHandler.inputChanged = true;
	}

	void OnMouseUp()
	{
		//Debug.Log ("pressed button" + thisButton.ToString());
		inputHandler.firePressed = false;
		inputHandler.inputChanged = true;
	}
}