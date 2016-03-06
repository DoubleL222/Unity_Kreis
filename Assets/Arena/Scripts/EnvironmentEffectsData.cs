using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentEffectsData
{
  public float minSpawnDuration;
  public float maxSpawnDuration;

  public int minNoOfEffects;
  public int maxNoOfEffects;

  // list of prefabs
  public List<GameObject> environmentEffects;

  public EnvironmentEffectsData()
  {
    environmentEffects = new List<GameObject>();
  }
}
