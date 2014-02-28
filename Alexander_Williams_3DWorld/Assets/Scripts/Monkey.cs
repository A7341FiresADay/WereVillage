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
	void Update () {

		/*var tar = followPath (points);
		characterController.transform.rotation = Quaternion.LookRotation ( Vector3.RotateTowards(transform.forward,  tar, 0.01f, 0) );

		var speed = 50;
		var new_dir = (tar.normalized + transform.forward * speed);*/
		characterController.SimpleMove ( steering.Seek(get_target_point())*5 );


	}

	int target_point = 0; 
	private Vector3 get_target_point(){
		if(Vector3.Distance(transform.position, points[target_point]) < 20){
			target_point = target_point+1 >= points.Count ? 0 : target_point+1;
		}
		return points[target_point];
	}

	private Vector3 followPath(List<Vector3> path)
	{
		var futurePos = (transform.forward * characterController.velocity.magnitude) + transform.position;
		
		Vector3 pathTarget = Vector3.zero;
		float distance = float.MaxValue;

		//Debug.DrawLine(transform.position, futurePos, Color.red);
		
		for(int i = 0; i < path.Count - 1; i++)
		{
			var p1 = path[i];
			var p2 = path[i+1];
			
			Vector3 point = getPointOnPath(futurePos, p1, p2) / 10;


			float d = Vector3.Distance(futurePos, point);
			
			if(d < distance) 
			{
				distance = d;
				pathTarget = point;
			}
		}
		
		//Debug.DrawLine(transform.position, pathTarget, Color.blue);

		target_marker.transform.position = pathTarget;

		if(distance > pathRadius) 
		{
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
