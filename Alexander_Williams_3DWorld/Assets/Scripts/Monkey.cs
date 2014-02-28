using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monkey : MonoBehaviour {

	List<Vector3> points;
	float pathRadius = 1;
	CharacterController characterController;
	Steering steering;

	GameObject target_marker;
	// Use this for initialization
	void Start () {
		target_marker = GameObject.Find ("path_marker");
		steering = gameObject.GetComponent<Steering> ();
		characterController = gameObject.GetComponent<CharacterController> ();
		points = new List<Vector3> ();
		var waypoints = GameObject.Find ("waypoints");
		for (var i = 0; i < waypoints.transform.childCount; i++) {
			points.Add(waypoints.transform.GetChild(i).transform.position);
		}
		/*for(var i = 0; i < Terrain.activeTerrain.terrainData.treeInstances.Length; i++){
			var t = Terrain.activeTerrain.terrainData.treeInstances[i];
			points.Add(t.position * 10);
		}*/
	}
	
	// Update is called once per frame
	int currentPoint = 0;
	void Update () {
		var target = points [currentPoint];
		var next_index = currentPoint + 1 >= points.Count ? 0 : currentPoint + 1;
		var next_target = points [next_index];


		//Debug.DrawLine (transform.position, target);
		//characterController.SimpleMove ( segmentFollow(target, next_target) );
		characterController.SimpleMove ( followPath(points) + transform.forward*2 );


		Debug.Log (Vector3.Distance (transform.position, target));
		if (Vector3.Distance (transform.position, target) < 10) {
			Debug.Log("moved to next target");
			currentPoint = currentPoint + 1 >= points.Count ? 0 : currentPoint + 1;
		}

	}


	private Vector3 followPath(List<Vector3> path)
	{
		var futurePos = (transform.forward * characterController.velocity.magnitude) + transform.position;
		
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

		target_marker.transform.position = pathTarget;
		
		if(distance > pathRadius) 
		{
			//Debug.Log(pathTarget+dir); 
			return steering.Seek(pathTarget);
		}
		else return Vector3.zero;
	}

	private Vector3 segmentFollow(Vector3 first, Vector3 next)
	{
		var futurePos = (transform.forward * characterController.velocity.magnitude) + transform.position;
		
		Vector3 pathTarget = Vector3.zero;
		Vector3 dir =  Vector3.zero;
		
		var p1 = first;
		var p2 = next;
		
		Vector3 point = getPointOnPath(futurePos, p1, p2);
		
		float d1 = Vector3.Distance(point, p1);
		float d2 = Vector3.Distance(point, p2);
		Vector3 line = p1 - p2;
		
		if(d1 + d2 > line.magnitude + 1) point = p2;
		
		float distance = Vector3.Distance(futurePos, point);
		
		pathTarget = point;
		dir = line;
		dir.Normalize();
		dir *= 5;
		
		
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
