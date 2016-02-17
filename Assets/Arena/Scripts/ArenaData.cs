using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ArenaData
{
	public List<RingData> rings;
	public SpawnData spawns;
	public PowerUpSpawnerData powerupSpawner;

	public ArenaData(){
		rings = new List<RingData> ();
	}

	public Vector2 getSpawnPosition(int numberOfPlayers, int player){
		Vector2 ret = new Vector2 ();
		List<SpawnData.SpawnPosition> positions = null;
		if (!spawns.spawns.TryGetValue (numberOfPlayers, out positions)) { //if number of players nonexistent select first greater
			int[] values = spawns.spawns.Keys.OrderBy(i => i).ToArray();
			for (int i = 0; i < values.Length; i++) {
				if (values [i] >= numberOfPlayers) {
					positions = spawns.spawns [values [i]];
					break;
				}
			}
		}
		if (positions == null) {
			return ret;
		}
		SpawnData.SpawnPosition sp = positions [player];
		ret.x = sp.angle;
		while (ret.x < 0)
			ret.x += 360;
		while (ret.x >= 360)
			ret.x -= 360;
		ret.x = sp.angle * PolarPhysicsObject.widthMultiplier * Mathf.PI / 180f;
		ret.y = (rings [sp.ring].size + rings [sp.ring + 1].size) / 2f;
		return ret;
	}
}
