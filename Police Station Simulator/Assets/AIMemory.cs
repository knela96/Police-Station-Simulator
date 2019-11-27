using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KnowledgeBit
{
	public KnowledgeBit(GameObject go)
	{
		value_go = go;
		timestamp = Time.time;
	}

	public Vector3 value_vec3;
	public GameObject value_go;
	public float timestamp;
	public bool is_memory = false;

	public float Elapsed()
	{
		return Time.time - timestamp;
	}
}

public class AIMemory : MonoBehaviour {

	public GameObject Cube;
	public Text Output;
	Dictionary<string, KnowledgeBit> Knowledge;

	// Update is called once per frame
	void PerceptionEvent (PerceptionEvent ev) {

		if(ev.type == global::PerceptionEvent.types.NEW)
		{
			AddBit(ev.go.name, ev.go);
		}
		else
		{
			BitToMemory(ev.go.name);
		}
	}

	// Use this for initialization
	void Start () {
		Knowledge = new Dictionary<string, KnowledgeBit>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Output.text = "";

		foreach(KeyValuePair<string, KnowledgeBit> KBit in Knowledge)
 		{
 			if(KBit.Value.is_memory == true)
 			{
				Output.text += string.Format("{0} = {1} [{2}]\n", KBit.Key, KBit.Value.value_vec3, KBit.Value.Elapsed());
				Cube.transform.position = KBit.Value.value_vec3;
 			} 
 			else
 			{
				Output.text += string.Format("{0} = {1}\n", KBit.Key, KBit.Value.value_go.transform.position);
				Cube.transform.position = KBit.Value.value_go.transform.position;
 			}
		}
	}

	void AddBit(string name, GameObject go)
	{
		KnowledgeBit KBit;
		if(Knowledge.TryGetValue(name, out KBit) == true)
		{
			KBit.value_go = go;
			KBit.timestamp = Time.time;
			KBit.is_memory = false;
		}
		else
			Knowledge.Add(name, new KnowledgeBit(go));
	}

	void BitToMemory(string name)
	{
		KnowledgeBit KBit;
		if(Knowledge.TryGetValue(name, out KBit) == true)
		{
			KBit.is_memory = true;
			KBit.value_vec3 = KBit.value_go.transform.position;
			KBit.value_go = null;
			KBit.timestamp = Time.time;
		}
	}
}
