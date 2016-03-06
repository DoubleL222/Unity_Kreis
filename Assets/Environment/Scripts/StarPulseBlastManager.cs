using UnityEngine;
using System.Collections;

public class StarPulseBlastManager : PolarPhysicsObject
{
  public Transform root;
  public float blastVelocity = 50.0f;

  private float lifeTime = 3.0f;
  private float timer = .0f;

  private Vector2 dir;

  void Start ()
  {
    StartUpdate();
    EndUpdate();

    dir = new Vector2(Random.Range(-1.0f, 1.0f), 1.0f);
    dir.Normalize();
    rigidbody.velocity = dir * blastVelocity;
  }

  void FixedUpdate ()
  {
    StartUpdate();

    if (rigidbody.velocity != (dir * blastVelocity))
      rigidbody.velocity = dir * blastVelocity;

    EndUpdate();

    timer += Time.fixedDeltaTime;
    if (timer > lifeTime)
      Destroy(root.gameObject);

  }
}
