using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeopleHandler : MonoBehaviour {

	[SerializeField] private Transform bargainPoint;
	private List<Vector2> _waypoints;
	
	private void OnEnable() {
		Transform[] children = transform.GetComponentsInChildren<Transform>();
		_waypoints = children.Select(transform1 => (Vector2) transform1.position).ToList();
	}

	void Update() {
		
	}
}