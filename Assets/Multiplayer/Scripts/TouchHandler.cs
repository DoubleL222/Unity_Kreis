using UnityEngine;
using System.Collections;

public class TouchHandler : MonoBehaviour
{
	public InputHandler inputHandler;
	public BoxCollider2D col;

	PlayerInput thisButton;

	void Start ()
	{
		switch (gameObject.tag) {
		case "buttonLeft":
			thisButton = PlayerInput.left;
			break;
		case "buttonRight":
			thisButton = PlayerInput.right;
			break;
		case "buttonUp":
			thisButton = PlayerInput.up;
			break;
		case "buttonShoot":
			thisButton = PlayerInput.shoot;
			break;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		Touch[] myTouches = Input.touches;
		for(int i = 0; i < Input.touchCount; i++)
		{
			Touch currentTouch = myTouches[i];

			if (col.bounds.Contains(currentTouch.position))
			{
		/*		switch (thisButton) {
				case "buttonLeft":
				//	;
					break;
				case "buttonRight":
					thisButton = PlayerInput.right;
					break;
				case "buttonUp":
					thisButton = PlayerInput.up;
					break;
				case "buttonShoot":
					thisButton = PlayerInput.shoot;
					break;
				}*/
			}
		}
	}
}
