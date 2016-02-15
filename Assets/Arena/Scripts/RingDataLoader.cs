using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Reflection;
using System.Linq;

public class RingDataLoader : MonoBehaviour
{
	private static Dictionary<String, Sprite> sprites;
	private static Dictionary<String, Type> ticks;
	private static Dictionary<String, Type> triggers;
	private static Dictionary<String, Type> collisions;

	static RingDataLoader(){
		sprites = new Dictionary<String, Sprite> ();
		ticks = GetDerivativesOfInterface<SegmentTickBehaviour> ();
		triggers = GetDerivativesOfInterface<SegmentTriggerBehaviour> ();
		collisions = GetDerivativesOfInterface<SegmentCollisionBehaviour> ();
		UnityEngine.Object[] textures = Resources.LoadAll ("Arena/Textures/Segments", typeof(Sprite));
		Debug.Log ("Unity loaded " + textures.Length + " assets.");
		for (int i = 0; i < textures.Length; i++) {
			if (textures [i].GetType () == typeof(Sprite)) {
				sprites.Add (textures [i].name, textures [i] as Sprite);
			}
		}
		String[] names = sprites.Keys.ToArray();
		Debug.Log ("Sprites loaded: ");
		for (int i = 0; i < names.Length; i++) {
			Debug.Log (names [i]);
		}
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
		Debug.Log("Loading");
		List<RingData> rd = Load ("Assets/Arena/Arenas/basic.txt");
		Debug.Log ("Loaded " + rd.Count + " rings");
		for (int i = 0; i < rd.Count; i++) {
			Debug.Log (rd [i].size);
			Debug.Log (rd [i].sprite);
			Debug.Log (rd [i].segmentCollisionBehaviours.Count);
			Debug.Log (rd [i].segmentTickBehaviours.Count);
			Debug.Log (rd [i].segmentTriggerBehaviours.Count);
		}
		Debug.Log ("Loaded");

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

	public static List<RingData> Load (string fileName)
	{
		List<RingData> ret = new List<RingData>();
		try {
			string line;
			StreamReader theReader = new StreamReader (fileName);
			using (theReader) {
				do {
					line = theReader.ReadLine ();
					if (line != null) {
						string[] entries = line.Split (' ');
						if(entries[0] == "ring"){			
							RingData ringData = loadRing(theReader);
							ret.Add(ringData);
						}
					}
				} while (line != null);
				theReader.Close ();
				return ret;
			}
		} catch (Exception e) {
			Console.WriteLine ("{0}\n", e.Message);
			return ret;
		}
	}
}
