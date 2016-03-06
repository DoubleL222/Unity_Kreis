using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Reflection;
using System.Linq;

public class ArenaDataLoader : MonoBehaviour
{
	private static Dictionary<String, Sprite> sprites;
	private static Dictionary<String, Type> ticks;
	private static Dictionary<String, Type> triggers;
	private static Dictionary<String, Type> collisions;

	public static Dictionary<String, ArenaData > arenas;

	static ArenaDataLoader(){
		sprites = new Dictionary<String, Sprite> ();
		ticks = GetDerivativesOfInterface<SegmentTickBehaviour> ();
		triggers = GetDerivativesOfInterface<SegmentTriggerBehaviour> ();
		collisions = GetDerivativesOfInterface<SegmentCollisionBehaviour> ();
		arenas = new Dictionary<string, ArenaData> ();
		UnityEngine.Object[] textures = Resources.LoadAll ("Textures/Segments", typeof(Sprite));
		//Debug.Log ("Unity loaded " + textures.Length + " assets.");
		for (int i = 0; i < textures.Length; i++) {
			if (textures [i].GetType () == typeof(Sprite)) {
				sprites.Add (textures [i].name, textures [i] as Sprite);
			}
		}/*
		String[] names = sprites.Keys.ToArray();
		Debug.Log ("Sprites loaded: ");
		for (int i = 0; i < names.Length; i++) {
			Debug.Log (names [i]);
		}*/
		/*
		String[] names = ticks.Keys.ToArray();
		Type[] classes = ticks.Values.ToArray();
		Debug.Log ("Ticks: ");
		for (int i = 0; i < names.Length; i++) {
			Debug.Log (names [i] + " " + classes [i]);
		}
		names = triggers.Keys.ToArray ();
		classes = triggers.Values.ToArray ();
		Debug.Log ("triggers: ");
		for (int i = 0; i < names.Length; i++) {
			Debug.Log (names [i] + " " + classes [i]);
		}
		names = collisions.Keys.ToArray ();
		classes = collisions.Values.ToArray ();
		Debug.Log ("collisions: ");
		for (int i = 0; i < names.Length; i++) {
			Debug.Log (names [i] + " " + classes [i]);
		}*/
		//Debug.Log("Loading");
		string[] filenames = null;
		filenames = Directory.GetFiles(Application.streamingAssetsPath + "/Arenas").Where(s => s.EndsWith(".txt")).ToArray();
		
		foreach(string filename in filenames){
			//Debug.Log(filename);
			ArenaData ad = Load (filename);
			int first = filename.LastIndexOf ('\\') + 1;
			int last = filename.LastIndexOf (".txt");
			arenas.Add(filename.Substring(first, last-first), ad);
			/*
			for (int i = 0; i < rd.Count; i++) {
				Debug.Log (rd [i].size);
				Debug.Log (rd [i].sprite);
				Debug.Log (rd [i].segmentCollisionBehaviours.Count);
				Debug.Log (rd [i].segmentTickBehaviours.Count);
				Debug.Log (rd [i].segmentTriggerBehaviours.Count);
			}*/
			/*
			if (ad.powerupSpawner != null) {
				Debug.Log ("Power up spawner data:");
				Debug.Log (ad.powerupSpawner.maxNumberOfPowerups);
				Debug.Log (ad.powerupSpawner.maxSpawnDuration);
				Debug.Log (ad.powerupSpawner.minSpawnDuration);
				Debug.Log (ad.powerupSpawner.powerUps.Count);
				for (int i = 0; i < ad.powerupSpawner.powerUps.Count; i++)
					Debug.Log (ad.powerupSpawner.powerUps [i].name);
				Debug.Log (ad.powerupSpawner.spawnDistances.Count);
				for(int i=0; i < ad.powerupSpawner.spawnDistances.Count; i++)
					Debug.Log (ad.powerupSpawner.spawnDistances[i]);
			} else {
				Debug.Log ("No power up spawner data:");
			}*/
		}
		Debug.Log ("Arena names");
		foreach (string s in arenas.Keys) {
			Debug.Log (s);
		}
	}
	 
	private static Dictionary<String, Type> GetDerivativesOfInterface<T>() where T : class
	{
		Dictionary<String, Type> ret = new Dictionary<String, Type>();
		foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => (typeof(T)).IsAssignableFrom(myType) && (typeof(T)) != myType)) {
			ret.Add(type.Name, type);
		}
		return ret;
	}

	private static object makeInstance(Type t, object[] args){
		ConstructorInfo[] constructors = t.GetConstructors ();
		foreach (ConstructorInfo ci in constructors) {
			ParameterInfo[] pi = ci.GetParameters ();
			if (pi.Length != args.Length)
				continue;
			object[] vals = new object[args.Length];
			try{
				for (int i = 0; i < args.Length; i++) {
					vals[i] = Convert.ChangeType(args[i], pi[i].ParameterType);
				}
				return ci.Invoke(vals);
			}catch(Exception e){
				Debug.Log (e.Message);
			}
		}
		return null;
	}
		
	private static RingData loadRing(StreamReader reader){
		RingData rd = new RingData ();
		string line;
		do {
			line = reader.ReadLine ();
			if (line != null) {
				line = line.Trim();
				string[] entries = line.Split (' ');
				if(entries[0] == "tick"){ //new tick behaviour
					Type t;
					if(ticks.TryGetValue(entries[1], out t)){
						SegmentTickBehaviour stb = makeInstance(t, entries.Skip(2).ToArray()) as SegmentTickBehaviour;
						if(stb != null)
							rd.segmentTickBehaviours.Add(stb);
					}					
				}else if(entries[0] == "trigger"){ //new trigger behaviour
					Type t;
					if(triggers.TryGetValue(entries[1], out t))
					{
						SegmentTriggerBehaviour stb = makeInstance(t, entries.Skip(2).ToArray()) as SegmentTriggerBehaviour;
						if(stb != null){
							rd.segmentTriggerBehaviours.Add(stb);
						}
					}
				}else if(entries[0] == "collision"){ //new collision behaviour
					Type t;
					if(collisions.TryGetValue(entries[1], out t)){
						SegmentCollisionBehaviour scb = makeInstance(t, entries.Skip(2).ToArray()) as SegmentCollisionBehaviour;
						if(scb != null){
							rd.segmentCollisionBehaviours.Add(scb);
						}	
					}
				}else if(entries[0] == "sprite"){ //new sprite
					Sprite s;
					if(sprites.TryGetValue(entries[1], out s)){
						rd.sprite = s;	
					}
				}else if(entries[0] == "size"){
					rd.size = float.Parse(entries[1]);
				}else if(entries[0] == "end"){
					return rd;
				}
			}
		} while (line != null);
		return rd;
	}

	private static SpawnData loadSpawns(StreamReader reader){
		SpawnData sd = new SpawnData ();
		string line;
		do {
			line = reader.ReadLine ();
			if (line != null) {
				line = line.Trim();
				string[] entries = line.Split (' ');
				if(entries[0] == "pc"){
					sd.spawns.Add(int.Parse(entries[1]), loadSpawn(reader));
				}else if(entries[0] == "end"){
					return sd;
				}	
			}
		} while (line != null);
		return sd;
	}

	private static List<SpawnData.SpawnPosition> loadSpawn(StreamReader reader){
		List<SpawnData.SpawnPosition> sd = new List<SpawnData.SpawnPosition> ();
		string line;
		do {
			line = reader.ReadLine ();
			if (line != null) {
				line = line.Trim();
				string[] entries = line.Split (' ');
				if(entries[0] == "p"){
					sd.Add(new SpawnData.SpawnPosition(int.Parse(entries[1]), float.Parse(entries[2])));
				}else if(entries[0] == "end"){
					return sd;
				}
			}
		} while (line != null);
		return sd;
	}

	public static PowerUpSpawnerData loadSpawnerData(StreamReader reader){
		PowerUpSpawnerData sd = new PowerUpSpawnerData ();
		string line;
		do {
			line = reader.ReadLine ();
			if (line != null) {
				line = line.Trim();
				string[] entries = line.Split (' ');
				if(entries[0] == "powerup"){
					sd.powerUps.Add(Resources.Load(entries[1]) as GameObject);
					sd.spawnDistances.Add(float.Parse(entries[2]));
				}else if(entries[0] == "end"){
					return sd;
				}
			}
		} while (line != null);
		return sd;
	}

  public static EnvironmentEffectsData loadEnvironmentEffectsData(StreamReader reader)
  {
    EnvironmentEffectsData ed = new EnvironmentEffectsData();
    string line;
    do
    {
      line = reader.ReadLine();
      if (line != null)
      {
        line = line.Trim();
        string[] entries = line.Split(' ');
        if (entries[0] == "effect")
        {
          ed.environmentEffects.Add(Resources.Load(entries[1]) as GameObject);
          ed.minNoOfEffects = int.Parse(entries[2]);
          ed.maxNoOfEffects = int.Parse(entries[3]);
        }
        else if (entries[0] == "end")
        {
          return ed;
        }
      }
    } while (line != null);
    return ed;
  }

  public static ArenaData Load (string fileName)
	{
		ArenaData ad = new ArenaData ();
		try {
			string line;
			StreamReader theReader = new StreamReader (fileName);
			using (theReader) {
				do {
					line = theReader.ReadLine ();
					if (line != null) {
						line = line.Trim();
						string[] entries = line.Split (' ');
						if(entries[0] == "ring"){
							ad.rings.Add(loadRing(theReader));
						}else if(entries[0] == "spawn"){
							ad.spawns = loadSpawns(theReader);
						}else if(entries[0] == "powerups"){
							ad.powerupSpawner = loadSpawnerData(theReader);
							ad.powerupSpawner.minSpawnDuration = float.Parse(entries[1]);
							ad.powerupSpawner.maxSpawnDuration = float.Parse(entries[2]);
							ad.powerupSpawner.maxNumberOfPowerups = int.Parse(entries[3]);
						}
            else if (entries[0] == "environmentEffects")
            {
              ad.environmentEffectsData = loadEnvironmentEffectsData(theReader);
              ad.environmentEffectsData.minSpawnDuration = float.Parse(entries[1]);
              ad.environmentEffectsData.maxSpawnDuration = float.Parse(entries[2]);
            }
          }
				} while (line != null);
				theReader.Close ();
				return ad;
			}
		} catch (Exception e) {
			Console.WriteLine ("{0}\n", e.Message);
			return ad;
		}
	}
}
