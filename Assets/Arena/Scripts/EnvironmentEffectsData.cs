using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentEffectsData
{
  public float minSpawnDuration;
  public float maxSpawnDuration;

  // list of prefabs
  public List<GameObject> environmentEffects;

  public EnvironmentEffectsData()
  {
    environmentEffects = new List<GameObject>();
  }
}
