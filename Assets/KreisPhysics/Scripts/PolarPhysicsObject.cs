using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;

public abstract class PolarPhysicsObject : MonoBehaviour{
	protected static readonly float widthMultiplier = 10f;
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

	private Collider2D[] collidercopiesleft;
	private Collider2D[] collidercopiesright;

	protected void Awake(){
		Vector3 tmp = physics_parent.transform.localScale;
		tmp.Scale (new Vector3(widthMultiplier, 1, 1));
		physics_parent.transform.localScale = tmp;
		oldscale = 1;
		oldVelocity = new Vector2 (0f, 0f);

		colliders = physics.GetComponents<Collider2D> ();
		collidercopiesleft = new Collider2D[colliders.Length];
		collidercopiesright = new Collider2D[colliders.Length];
		for (int i = 0; i < colliders.Length; i++) {
			collidercopiesleft [i] = GetCopyOf (physics.AddComponent (colliders[i].GetType ()), colliders [i]);
			collidercopiesleft [i].enabled = false;

			collidercopiesright [i] = GetCopyOf (physics.AddComponent (colliders[i].GetType ()), colliders [i]);
			collidercopiesright [i].enabled = false;

		}
	}

	protected void StartUpdate(){
		if (physics != null) {
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
		if (rigidbody != null) {
			Vector2 tmpvel = rigidbody.velocity;
			tmpvel -= oldVelocity;
			tmpvel.Scale (new Vector2 (oldscale, oldscale));
			oldVelocity += tmpvel;
			oldVelocity.Scale (new Vector2 (1f / oldscale, 1f));
			oldVelocity.x = Mathf.Clamp (oldVelocity.x, -maxHorizontalSpeed * widthMultiplier, maxHorizontalSpeed * widthMultiplier);
			oldVelocity.y = Mathf.Clamp (oldVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);
			if(!UtilityScript.isVector2NanOrInf(oldVelocity))
				rigidbody.velocity = oldVelocity;
			oldscale = scaleMultiplier / rigidbody.position.y;
		}
		if (physics != null && mesh != null) {
			Vector3 pos = physics.transform.position;

			float angle = pos.x / widthMultiplier;
			float distance = pos.y;

			float mx = distance * Mathf.Cos (angle);
			float my = distance * Mathf.Sin (angle);

			mesh.transform.position = new Vector3 (mx, my, 0);
			mesh.transform.rotation = Quaternion.Euler (0, 0, (angle) * 180f / Mathf.PI + 90f);
		}
		for(int i=0; i < colliders.Length; i++){
			Collider2D col = colliders [i];
			Bounds bounds = col.bounds;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			if (min.x < -Mathf.PI * widthMultiplier) {
				collidercopiesright [i].enabled = true;
				Vector2 offset = collidercopiesright [i].offset;
				offset.x = colliders[i].offset.x + (2 * Mathf.PI * widthMultiplier) / (physics.transform.lossyScale.x);
				collidercopiesright [i].offset = offset;
			} else {
				collidercopiesright [i].enabled = false;
			}

			if (max.x > Mathf.PI * widthMultiplier) {
				collidercopiesleft [i].enabled = true;
				Vector2 offset = collidercopiesleft [i].offset;
				offset.x = colliders[i].offset.x - (2 * Mathf.PI * widthMultiplier) / (physics.transform.lossyScale.x);
				collidercopiesleft [i].offset = offset;
			} else {
				collidercopiesleft [i].enabled = false;
			}
		}
	}


	protected void EndUpdate(){
		if (rigidbody != null) {
			oldVelocity = rigidbody.velocity;
			//Debug.Log ("Actual velocity: " + rigidbody.velocity);
			oldVelocity.Scale (new Vector2 (oldscale, 1f));
			if(!UtilityScript.isVector2NanOrInf(oldVelocity))
				rigidbody.velocity = oldVelocity;
			
			//Debug.Log ("Applied velocity: " + rigidbody.velocity);

			//Vector3 tmpvec = rigidbody.velocity;
			//tmpvec.Scale (new Vector2 (rigidbody.position.y, 1f));
			//Debug.Log ("Velocity given position: " + tmpvec);

			float oldyscale = rigidbody.transform.localScale.y;
			if(!UtilityScript.isVector2NanOrInf(new Vector2(oldscale, oldyscale)))
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
