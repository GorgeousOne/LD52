using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerHandler : MonoBehaviour {

	private List<Spawner> _spawners;
	
	// Start is called before the first frame update
	void Start() {
		_spawners = FindObjectsOfType<Spawner>().ToList();
	}

	public Spawner GetClosestSpawner(Vector3 pos,ref float minDistSq) {
		Spawner closest = null;
		
		foreach (Spawner spawner in _spawners) {
			float distSq = (pos - spawner.transform.position).sqrMagnitude;
			
			if (distSq < minDistSq) {
				minDistSq = distSq;
				closest = spawner;
			}
		}
		return closest;
	}
}
