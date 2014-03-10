using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class agent : MonoBehaviour {

	public GameObject target;
	private Vector3 short_term_target;

	// Use this for initialization
	void Start () {
		short_term_target = nearist_node().transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(move_to_short_term_target ()) {
			var nearist_node = this.nearist_node();
			List<a_star_node> a_star_path = a_star (new a_star_node(nearist_node, Vector3.Distance(nearist_node.transform.position, target.transform.position)), new a_star_node(target, 0));
			int target_index = (a_star_path.Count > 1) ? 1 : 0;
			short_term_target = a_star_path[target_index].node.transform.position;
		}
		Debug.DrawLine (transform.position, target.transform.position);
		
		Debug.DrawLine (transform.position, short_term_target, Color.gray);
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
		float d = Vector3.Distance (transform.position, short_term_target);
		Debug.Log(d);
		return d < 5;
	}

	public GameObject nearist_node(){
		float min_dist = float.MaxValue;
		GameObject nearist = nodes()[0];
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



	public List<a_star_node> a_star(a_star_node start, a_star_node goal)
	{
		List<a_star_node> closedset = new List<a_star_node> ();
		List<a_star_node> openset = new List<a_star_node>();
		openset.Add (start);
		Dictionary<a_star_node, float> g_score = new Dictionary<a_star_node, float> ();
		g_score [start] = start.f_score;
	
		Dictionary<a_star_node, a_star_node> came_from = new Dictionary<a_star_node, a_star_node>();



		while (openset.Count > 0) {
			a_star_node current = lowest_f_score(openset);

			if(current.node == goal.node){

				return reconstruct_path(came_from, goal);
			}

			openset.Remove(current);
			closedset.Add(current);

			foreach(a_star_node neighbor in neighbors(current)){
				if(closedset.IndexOf(neighbor) != -1){
					continue;
				}
				float tenative_score = get_score(g_score, current, 0) + neighbor.f_score;

				if(openset.IndexOf(neighbor) == -1 || tenative_score < get_score(g_score, neighbor, 0)){
					came_from[neighbor] = current;
					g_score[neighbor] = tenative_score;
					if(openset.IndexOf(neighbor) == -1){
						openset.Add(neighbor);
					}

				}
			}

		}




		List<a_star_node> to_return = new List<a_star_node> ();
		to_return.Add (goal);
		return to_return;


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
		int index = this.nodes().IndexOf(node.node);


		List<a_star_node> nodes = new List<a_star_node> ();
		foreach (var edge in this.nodes()[index].GetComponent<node>().edges) {
			nodes.Add(new a_star_node(edge.to, edge.weight + Vector3.Distance(node.node.transform.position, edge.to.transform.position)));
		}
		return nodes;
	}
				                                                            
	public float get_score(Dictionary<a_star_node, float> score, a_star_node key, float fail){
		float out_val = 0;
		if( score.TryGetValue(key, out out_val) ){
			return out_val;
		}
		return 0;

	}

	public List<a_star_node> reconstruct_path(Dictionary<a_star_node, a_star_node> came_from, a_star_node current_node){

		if (came_from.ContainsKey (current_node)) {
				List<a_star_node> to_return = reconstruct_path(came_from, came_from[current_node]);
				to_return.Add(current_node);
				return to_return;
		} else {
			List<a_star_node> to_return = new List<a_star_node> ();
			to_return.Add (current_node);
			return to_return;
		}
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












