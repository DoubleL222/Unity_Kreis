using System;
using System.Collections.Generic;

public class SpawnData
{
	public class SpawnPosition{
		public int ring;
		public float angle;

		public SpawnPosition(int ring, float angle){
			this.ring = ring;
			this.angle = angle;
		}
	}

	public Dictionary<int, List<SpawnPosition> > spawns;
	public SpawnData ()
	{
		spawns = new Dictionary<int, List<SpawnPosition>> ();
	}
}

