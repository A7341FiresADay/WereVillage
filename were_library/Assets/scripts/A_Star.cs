
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class A_Star {

	a_star_node agent;
	List<a_star_node> nodes;
	a_star_node target;
	List<a_star_node> path;

	public A_Star(){
	}

	public void set_target(GameObject g_target)
	{
		target = new a_star_node (g_target);

	}

	public void load_nodes(List<GameObject> g_nodes)
	{
		nodes = new List<a_star_node> ();
		foreach (GameObject g_node in g_nodes) 
		{
			nodes.Add(new a_star_node(g_node));
		}
	}

	public Vector3 short_term_target(GameObject _agent) //switch to storing later
	{
		agent = nearist_node_to(nodes, _agent);
		if (path == null) {
			path = a_star(agent, target);
		}

		return nearist_node_to( path, _agent).node.transform.position;

	}
	


	//---------------------------private--------------------------------------


	
	private List<a_star_node> a_star(a_star_node start, a_star_node goal)
	{
		List<a_star_node> closedset = new List<a_star_node> ();
		List<a_star_node> openset = new List<a_star_node>();
		openset.Add (start);
		Dictionary<a_star_node, float> g_score = new Dictionary<a_star_node, float> ();
		Dictionary<a_star_node, float> f_score = new Dictionary<a_star_node, float> ();
		g_score [start] = 0;
		f_score [start] = get_heuristic_cost (start);

		
		Dictionary<a_star_node, a_star_node> came_from = new Dictionary<a_star_node, a_star_node>();
		
		
		
		while (openset.Count > 0) 
		{
			a_star_node current = lowest_f_score(openset, f_score);
			
			if(current.node == goal.node){
				return reconstruct_path(came_from, goal);
			}
			
			openset.Remove(current);
			closedset.Add(current);
			
			foreach(a_star_node neighbor in neighbors(current))
			{
				if(closedset.IndexOf(neighbor) != -1)
				{
					continue;
				}
				float tenative_score = get_score(g_score, current) + 1;//change 1 to edge-weight
				
				if(openset.IndexOf(neighbor) == -1 || tenative_score < get_score(g_score, neighbor))
				{
					came_from[neighbor] = current;
					g_score[neighbor] = tenative_score;
					f_score[neighbor] = g_score[neighbor] + get_heuristic_cost(neighbor);
					if(openset.IndexOf(neighbor) == -1)
					{
						openset.Add(neighbor);
					}
					
				}
			}
			
		}

		List<a_star_node> to_return = new List<a_star_node> ();
		to_return.Add (goal);
		return to_return;
	}

	private a_star_node lowest_f_score(List<a_star_node> set, Dictionary<a_star_node, float> f_score) //SHOULD INCLUDE F (THIS IS JUST G SCORE
	{
		a_star_node to_return = set [0];
		
		
		foreach(a_star_node n in set){
			float score_1 = float.MaxValue;
			f_score.TryGetValue(n, out score_1);
			float score_2 = float.MaxValue;
			f_score.TryGetValue(to_return, out score_2);

			if(score_1 < score_2){
				to_return = n;
			}
		}
		
		return to_return;
	}
	
	private List<a_star_node> neighbors(a_star_node node)
	{
		foreach (var edge in node.edges) {
			nodes.Add(new a_star_node(edge.to));
		}
		return nodes;
	}
	
	private float get_score(Dictionary<a_star_node, float> score, a_star_node key){
		float out_val = 0;
		if( score.TryGetValue(key, out out_val) ){
			return out_val;
		}
		return 0;
		
	}


	
	
	private a_star_node nearist_node_to(List<a_star_node> these_nodes, GameObject close_to){
		float min_dist = float.MaxValue;
		a_star_node nearist = these_nodes[0];
		foreach (a_star_node a_node in these_nodes) {
			float max_dist = Vector3.Distance(a_node.node.transform.position, close_to.transform.position);
			if(max_dist <= min_dist){
				min_dist = max_dist;
				nearist = a_node;
			}
		}
		return nearist;
	}

	
	private List<a_star_node> reconstruct_path(Dictionary<a_star_node, a_star_node> came_from, a_star_node current_node){
		
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


	private float get_heuristic_cost(a_star_node n){
		
		if(target.node != null){
			return Vector3.Distance(n.node.transform.position, target.node.transform.position);
		} else {
			return 0;
		}
	}

}




public struct a_star_node{
	public a_star_node(GameObject _node){
		node = _node;
	}
	public GameObject node;
	public List<edge> edges {get {
			return node.GetComponent<node>().edges;
		}}
}



