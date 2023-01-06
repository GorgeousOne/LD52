using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	[SerializeField] private float interactDist = .75f;
	[SerializeField] private SpawnerHandler spawnerHandler;
	public Item heldItem;
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKey(KeyCode.E)) {
			Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(transform.position, interactDist);

			if (interactedSpawner != null) {
				
			}
		}
	}
}
