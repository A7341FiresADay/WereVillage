using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class agent : MonoBehaviour {

	public A_Star AStar;

	public GameObject target;

	// Use this for initialization
	void Start () {
		AStar = new A_Star ();
		choose_target (GameObject.Find ("map").GetComponent<map_layout> ().posts);
	
		AStar.load_nodes (GameObject.Find ("map").GetComponent<map_layout> ().posts);

		
	}
	
	// Update is called once per frame
	void Update () {	
		Vector3 stt =  AStar.short_term_target ( gameObject ) ;
		transform.position = Vector3.MoveTowards (transform.position, stt, 0.5f);

		Debug.DrawLine (transform.position, target.transform.position, Color.blue);
		Debug.DrawLine (transform.position, stt, Color.gray);


	}

	public void choose_target(List<GameObject> nodes){
		target = nodes [(int)Random.Range (0, nodes.Count - 1)];
		AStar.set_target( target );
		
	}
	


	public void load_nodes(List<GameObject> nodes){
		AStar.load_nodes (nodes);
	}


}










