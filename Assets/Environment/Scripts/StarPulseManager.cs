using UnityEngine;
using System.Collections;

public class StarPulseManager : PolarPhysicsObject
{
  public GameObject starPulseBlast;
  public Transform root;

  [HideInInspector]
  public int minNoOfBlasts;
  [HideInInspector]
  public int maxNoOfBlasts;

  private float duration = .0f;
  private float timer = .0f;

	// Use this for initialization
	void Start ()
  {
    StartUpdate();
    EndUpdate();

    ParticleSystem PS = mesh.GetComponent<ParticleSystem>();
    duration = PS.startLifetime;
  }
	
	// Update is called once per frame
	void Update ()
  {
    timer += Time.deltaTime;

    if (timer > duration)
    {
      int noOfBlasts = Random.Range(minNoOfBlasts, maxNoOfBlasts + 1);
      CamShakeManager.PlayShake(0.5f, noOfBlasts * 0.5f);
      for (int i = 0; i < noOfBlasts; i++)
      {
        GameObject starPulse = MonoBehaviour.Instantiate(starPulseBlast, physics.transform.position, new Quaternion()) as GameObject;
      }
      Destroy(gameObject);
    }
	}
}
