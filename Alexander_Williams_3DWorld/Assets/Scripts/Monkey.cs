using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monkey : MonoBehaviour {

	List<Vector3> points;
	float pathRadius = 1;
	CharacterController characterController;
	Steering steering;
	// Use this for initialization
	void Start () {
		steering = gameObject.GetComponent<Steering> ();
		characterController = gameObject.GetComponent<CharacterController> ();
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
		transform.position = followPath (points);
	}



	private Vector3 followPath(List<Vector3> path)
	{
		var velocity = 10;
		var futurePos = (transform.forward * velocity) + transform.position;
		
		Vector3 pathTarget = Vector3.zero;
		Vector3 dir =  Vector3.zero;
		float distance = float.MaxValue;
		
		for(int i = 0; i < path.Count - 1; i++)
		{
			var p1 = path[i];
			var p2 = path[i+1];
			
			Vector3 point = getPointOnPath(futurePos, p1, p2);
			
			float d1 = Vector3.Distance(point, p1);
			float d2 = Vector3.Distance(point, p2);
			Vector3 line = p1 - p2;
			
			if(d1 + d2 > line.magnitude + 1) point = p2;
			
			float d = Vector3.Distance(futurePos, point);
			
			if(d < distance) 
			{
				distance = d;
				pathTarget = point;
				dir = line;
				dir.Normalize();
				dir *= 5;
			}
		}
		
		if(distance > pathRadius) 
		{
			//Debug.Log(pathTarget+dir); 
			return steering.Seek(pathTarget);
		}
		else return Vector3.zero;
	}

	public Vector3 getPointOnPath(Vector3 p, Vector3 a, Vector3 b)
	{
		Vector3 ap = a - p;
		Vector3 ab = b - a;
		ab.Normalize();
		ab *= Vector3.Dot(ap, ab);
		return a + ab;
	}
}
