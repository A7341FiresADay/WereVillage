using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monkey : MonoBehaviour {

	List<Vector3> points;
	// Use this for initialization
	void Start () {
		points = new List<Vector3> ();
		for(var i = 0; i < Terrain.activeTerrain.terrainData.treeInstances.Length; i++){
			var t = Terrain.activeTerrain.terrainData.treeInstances[i];
			points.Add(t.position * 10);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("p:" + points [0]);
		Debug.Log ("m:" + transform.position);
		Vector3.MoveTowards(transform.position, points[0], 2);
	}
}
