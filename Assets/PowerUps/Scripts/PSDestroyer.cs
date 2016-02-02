using UnityEngine;
using System.Collections;

public class PSDestroyer : MonoBehaviour
{
  private ParticleSystem PS;

  public void Start()
  {
    PS = GetComponent<ParticleSystem>();
  }

  public void Update()
  {
    if (PS && !PS.IsAlive())
      Destroy(gameObject);
  }
}