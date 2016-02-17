using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnerData
{
	public float minSpawnDuration;
	public float maxSpawnDuration;
	public int maxNumberOfPowerups;
	// list of prefabs
	public List<GameObject> powerUps;
	// list of distances
	public List<float> spawnDistances;

	public PowerUpSpawnerData ()
	{
		powerUps = new List<GameObject> ();
		spawnDistances = new List<float> ();
	}
}
