using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	[SerializeField] private float interactDist = .75f;
	[SerializeField] private SpawnerHandler spawnerHandler;
    [SerializeField] private ItemHandler itemHandler;
    public Item heldItem = null;
	
	// Update is called once per frame
	void Update() {

		if (Input.GetKeyUp(KeyCode.E)) {
			Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(transform.position, interactDist);
			Item interactedItem = itemHandler.GetClosestItem(transform.position, interactDist,heldItem);
            if (interactedItem != null && heldItem == null)
            {
                heldItem = interactedItem;
            }
            else if (interactedSpawner != null) {
                heldItem = interactedSpawner.GetItem();
            }
            else if (heldItem != null)
            {
				dropItem();
            }
        }
        if (heldItem != null)
        {
			heldItem.gameobject.transform.position = transform.position;
        }
    }
	private void dropItem(){
		heldItem = null;
	}
}
