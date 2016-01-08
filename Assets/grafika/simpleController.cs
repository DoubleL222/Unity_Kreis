using UnityEngine;
using System.Collections;

public class simpleController : MonoBehaviour {
    Rigidbody2D rb;
    public float movementSpeed = 2;
	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey("w"))
        {
            rb.velocity = new Vector2(rb.velocity.x, movementSpeed*4.0f);
        }
        else if (Input.GetKey("s"))
        {
            rb.velocity = new Vector2(rb.velocity.x, movementSpeed * -4.0f);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        if (Input.GetKey("a"))
        {
            rb.velocity = new Vector2(movementSpeed * -4.0f, rb.velocity.y);
        }
        else if (Input.GetKey("d"))
        {
            rb.velocity = new Vector2(movementSpeed * 4.0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
}
