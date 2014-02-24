using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {
	float wanderRandom = 0; //current position on projected circle
	public float wanderRate = 0.1f; //rate at which point on circle moves
	public int wanderRadius = 50; //radius of projected circle for wander
	public int wanderDistance = 50; //distance from character to projected circle

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	Vector3 wander() {
		wanderRandom += Random.Range(-wanderRate, wanderRate); //move the point on the circle to a random point within the rate
		float wanderAngle = wanderRandom * (Mathf.PI * 2); //get angle of point on circle
		return new Vector3(this.transform.position.x + (this.transform.forward.x * wanderDistance) +
		                   (wanderRandom * Mathf.Cos(wanderAngle)), 150,
		                   this.transform.position.y + (this.transform.forward.y * wanderDistance) +
		                   (wanderRandom * Mathf.Sin(wanderAngle))); //return vector of current position + forward vector * projected circle distance +
								//position of current point on project circle
	}
}
