using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class agent : MonoBehaviour {

	public GameObject target;
	private Vector3 short_term_target;

	// Use this for initialization
	void Start () {
		short_term_target = nearist_node ().transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(move_to_short_term_target ()) {
			short_term_target = a_star (gameObject, target);
		}
		Debug.DrawLine (transform.position, target.transform.position);
	}

	public void choose_target(){
		int random_node_number = (int)Random.Range(0, nodes().Count-1);
		GameObject target_node = nodes()[random_node_number];
		target = target_node;
		
	}


	public void move_towards(GameObject tar){

	}
	public bool move_to_short_term_target (){
		transform.position = Vector3.MoveTowards (transform.position, short_term_target, 0.5f);
		return (Vector3.Distance (transform.position, short_term_target) < 5);
	}

	public GameObject nearist_node(){
		float min_dist = float.MaxValue;
		GameObject nearist = new GameObject ();
		foreach (GameObject node in nodes()) {
			float max_dist = Vector3.Distance(node.transform.position, transform.position);
			if(max_dist <= min_dist){
				min_dist = max_dist;
				nearist = node;
			}
		}
		return nearist;
	}


	public List<GameObject> nodes(){
		return GameObject.Find("map").GetComponent<map_layout>().posts;
	}



	public Vector3 a_star(GameObject start, GameObject goal)
	{
		List<a_star_node> closedset = new List<a_star_node> ();
		List<a_star_node> openset = new List<a_star_node>();
		openset.Add (new a_star_node(start, 0));



		while (openset.Count > 0) {
			a_star_node current = lowest_f_score(openset);
			if(current.node == goal){
				//recunstruct path
			}

			openset.Remove(current);
			closedset.Add(current);

			foreach(a_star_node neightbor in neighbors(current)){
				if(closedset.IndexOf(neighbors) != -1){

				}
			}


		}


		return short_term_target;


	}


	public a_star_node lowest_f_score(List<a_star_node> set) //SHOULD INCLUDE F (THIS IS JUST G SCORE
	{
		a_star_node to_return = set [0];


		foreach(a_star_node n in set){
			if(n.f_score < to_return.f_score){
				to_return = n;
			}
		}

		return to_return;
	}

	public List<a_star_node> neighbors(a_star_node node){
		int index = nodes ().IndexOf (node.node);
		List<a_star_nodes> nodes = new List<a_star_node> ();
		foreach (var edge in nodes ()[index].GetComponent<node>().edges) {
			nodes.Add(new a_star_node(edge.to, edge.weight));
		}
		return nodes;
	}


}


public struct a_star_node{
	public a_star_node(GameObject _node, float _f_score){
		node = _node;
		f_score = _f_score;
	}
	public GameObject node;
	public float f_score;
}












