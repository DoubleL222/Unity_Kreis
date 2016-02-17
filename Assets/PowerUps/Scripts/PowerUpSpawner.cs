﻿using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawner
{
	// powerups span at random intervals from min to max duration
	public float minSpawnDuration;
	public float maxSpawnDuration;
	public int maxNumberOfPowerups;

	// list of prefabs
	public List<GameObject> powerUps;
	// list of distances
	public List<float> spawnDistances;

	// switch for enabling/disabling the powerup spawner
	public bool spawnPowerups = false;

	private float spawnDuration;
	private float timer = .0f;
	private int noOfPowerUps = 0;
	private int noOfDistances = 0;

	// GM
	private GameManager GM;
	public PowerUpSpawner(){
		powerUps = new List<GameObject> ();
		spawnDistances = new List<float> ();
		maxNumberOfPowerups = 0;
	}

	public PowerUpSpawner (GameManager GM, List<GameObject> powerUps, List<float> spawnDistances, float minSpawnDuration, float maxSpawnDuration, int maxNumberOfPowerups)
	{
		this.GM = GM;
		this.powerUps = powerUps;
		this.spawnDistances = spawnDistances;
		this.minSpawnDuration = minSpawnDuration;
		this.maxSpawnDuration = maxSpawnDuration;
		this.maxNumberOfPowerups = maxNumberOfPowerups;
		spawnDuration = Random.Range (minSpawnDuration, maxSpawnDuration);
		noOfPowerUps = powerUps.Count;
		noOfDistances = spawnDistances.Count;
	}

	public void Update ()
	{
		if (spawnPowerups == true && GM.activePowerUps.Count < maxNumberOfPowerups) {
			timer += Time.deltaTime;

			if (timer > spawnDuration) {
				// spawn new powerup
				int randomPowerup = Random.Range (0, noOfPowerUps);

				// random angle
				float angle = Random.Range (-10.0f * Mathf.PI, 10.0f * Mathf.PI);

				// random distance
				int randomDistance = Random.Range (0, noOfDistances);
				Vector3 pos = new Vector3 (angle, spawnDistances [randomDistance], 0.0f);

				GameObject powerup = MonoBehaviour.Instantiate (powerUps [randomPowerup], pos, new Quaternion ()) as GameObject;
				PowerUpManager PUM = powerup.GetComponentInChildren<PowerUpManager> ();
				GM.activePowerUps.Add (PUM);

				// reset timer
				spawnDuration = Random.Range (minSpawnDuration, maxSpawnDuration);
				timer = .0f;
			}
		}
	}
}
