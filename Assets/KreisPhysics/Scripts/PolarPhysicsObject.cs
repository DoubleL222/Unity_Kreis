using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;

/// <summary>
/// An object that has physics in polar coordinates and meshes in cartesian coordinates
/// </summary>
public abstract class PolarPhysicsObject : MonoBehaviour{
	public static readonly float widthMultiplier = 10f;
	protected static float scaleMultiplier = Mathf.PI*2;
	protected static float maxHorizontalSpeed = 5f;
	protected static float maxVerticalSpeed = 5f;

	protected Vector2 oldVelocity;
	protected float oldscale;

	public GameObject mesh;
	public GameObject physics;
	public GameObject physics_parent;
	public Rigidbody2D rigidbody;

	private Collider2D[] colliders;

	private Collider2D[] collidercopies;


	protected void Awake(){
		
		Vector3 tmp = physics_parent.transform.localScale;
		tmp.Scale (new Vector3(widthMultiplier, 1, 1));
		physics_parent.transform.localScale = tmp;
		oldscale = 1;
		oldVelocity = new Vector2 (0f, 0f);

		colliders = physics.GetComponents<Collider2D> ();
		collidercopies = new Collider2D[colliders.Length];	//Copies of colliders for use when the original collider is on 
																//the edge of the ring and needs to apply to the other side too 
																//(eg a collider at 359 degrees needs to affect a collider at 1 degree)
		for (int i = 0; i < colliders.Length; i++) {
			collidercopies [i] = GetCopyOf (physics.AddComponent (colliders[i].GetType ()), colliders [i]);
			collidercopies [i].enabled = false;
		}
	}
	/// <summary>
	/// This needs to be the first function call in the FixedUpdate of PolarPhysicsObjects!
	/// </summary>
	protected void StartUpdate(){
		
		if (physics != null) { //if this object has physics that moved beyond a whole circle (eg +181 or -181 degrees) it needs to be moved by +-360 degrees
			if (physics.transform.position.x > Mathf.PI * widthMultiplier) {
				Vector3 tmp = physics.transform.position;
				tmp.x -= 2 * Mathf.PI * widthMultiplier;
				physics.transform.position = tmp;
			} else if (physics.transform.position.x < -Mathf.PI * widthMultiplier) {
				Vector3 tmp = physics.transform.position;
				tmp.x += 2 * Mathf.PI * widthMultiplier;
				physics.transform.position = tmp;
			}
		}
		if (rigidbody != null) { //if this object has physics unscale the velocity and scale so you don't need to scale when adding to it
			Vector2 tmpvel = rigidbody.velocity;
			tmpvel -= oldVelocity;
			tmpvel.Scale (new Vector2 (oldscale, oldscale));
			oldVelocity += tmpvel;
			oldVelocity.Scale (new Vector2 (1f / oldscale, 1f));
			oldVelocity.x = Mathf.Clamp (oldVelocity.x, -maxHorizontalSpeed * widthMultiplier, maxHorizontalSpeed * widthMultiplier);
			oldVelocity.y = Mathf.Clamp (oldVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);
			//if(!UtilityScript.isVector2NanOrInf(oldVelocity))
				rigidbody.velocity = oldVelocity;
			oldscale = scaleMultiplier / rigidbody.position.y;
		}
		if (physics != null && mesh != null) { //if it has a mesh and physics it synchronizes the positions
			float angle = physics.transform.position.x / widthMultiplier;
			mesh.transform.position = UtilityScript.transformToCartesian (physics.transform.position);
			mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);
		}
		for(int i=0; i < colliders.Length; i++){ //enables the 'modulo' colliders if they need be enabled
			Collider2D col = colliders [i];
			Bounds bounds = col.bounds;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			Boolean mincond = min.x < -Mathf.PI * widthMultiplier, 
					maxcond = max.x > Mathf.PI * widthMultiplier;
			if (mincond || maxcond) {
				collidercopies [i].enabled = true;
				Vector2 offset = collidercopies [i].offset;
				if(mincond)
					offset.x = colliders[i].offset.x + (2 * Mathf.PI * widthMultiplier) / (physics.transform.lossyScale.x);
				else
					offset.x = colliders[i].offset.x - (2 * Mathf.PI * widthMultiplier) / (physics.transform.lossyScale.x);
				collidercopies [i].offset = offset;
			} else {
				collidercopies [i].enabled = false;
			}
		}
	}

	/// <summary>
	/// This needs to be the last function call in FixedUpdate of PolarPhysicsObjects!
	/// </summary>
	protected void EndUpdate(){
		if (rigidbody != null) { //if this object has a rigidbody it scales it back down again to proper size
			oldVelocity = rigidbody.velocity;
			oldVelocity.Scale (new Vector2 (oldscale, 1f));
			//if(!UtilityScript.isVector2NanOrInf(oldVelocity))
				rigidbody.velocity = oldVelocity;
			float oldyscale = rigidbody.transform.localScale.y;
			//r2NanOrInf(new Vector2(oldscale, oldyscale)))
				rigidbody.transform.localScale = new Vector3 (oldscale, oldyscale, oldscale);
		}
	}


	/*
	 * Courtesy of http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
	 * */
	private T GetCopyOf<T>(Component comp, T other) where T : Component
	{
		Type type = comp.GetType();
		if (type != other.GetType()) return null; // type mis-match
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach (var pinfo in pinfos) {
			if (pinfo.CanWrite) {
				try {
					pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
				}
				catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}
		FieldInfo[] finfos = type.GetFields(flags);
		foreach (var finfo in finfos) {
			finfo.SetValue(comp, finfo.GetValue(other));
		}
		return comp as T;
	}
}
