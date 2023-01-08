using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	//dont look at this
	[SerializeField] private float interactDist = 1.00f;
	[SerializeField] private SpawnerHandler spawnerHandler;
	[SerializeField] private ItemHandler itemHandler;
	[SerializeField] private PlotHandler plotHandler;
	[SerializeField] private WaterHandler waterHandler;
	[SerializeField] private Bucket bucketHandler;

	public Item heldItem;
	public Bucket heldBucket;
	private List<Transform> proximity = new();

	[SerializeField] private Transform indicator;
	Renderer script_renderer;

	void Start() {
		script_renderer = indicator.GetComponent<Renderer>();
		script_renderer.enabled = false;
	}

	// pls dont
	void Update() {
		float distancesqr = interactDist * interactDist;
		Bucket bucket = null;
		PlotContent interactedPlot = plotHandler.GetClosestPlot(transform.position, ref distancesqr);
		Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(transform.position, ref distancesqr);
		Item interactedItem = itemHandler.GetClosestItem(transform.position, heldItem, ref distancesqr);
		Water interactedWater = waterHandler.GetClosestWater(transform.position, ref distancesqr);

		if (heldBucket == null) {
			bucket = bucketHandler.distancesqr(transform.position, ref distancesqr);
		}

		_ToggleIndicator(interactedItem, bucket, interactedPlot, interactedSpawner, interactedWater);

		if (Input.GetKeyUp(KeyCode.E)) {
			_HandleE(interactedItem, bucket, interactedPlot, interactedSpawner, interactedWater);
		}
		else if (Input.GetKeyUp(KeyCode.Q)) {
			_DropItem();
		}
	}

	private void _ToggleIndicator(Item interactedItem, Bucket bucket, PlotContent interactedPlot,
		Spawner interactedSpawner, Water interactedWater) {
		bool newState = true;
		
		if (interactedItem != null) {
			indicator.position = interactedItem.gameobject.transform.position;
		}
		else if (bucket != null) {
			indicator.position = bucket.transform.position;
		}
		else if (interactedPlot != null) {
			indicator.position =
				interactedPlot.transform.position + Vector3.up * 0.5f +
				Vector3.right *
				0.5f; // parent.TransformPoint(interactedPlot.transform.position) + Vector3.up * 0.5f + Vector3.right * 0.5f;
		}
		else if (interactedSpawner != null) {
			indicator.position = interactedSpawner.transform.position;
		} else if (interactedWater) {
			indicator.position = new Vector2(interactedWater.transform.position.x, transform.position.y + 0.5f);
		}
		else {
			newState = false;
		}
		script_renderer.enabled = newState;
	}

	private void _HandleE(Item interactedItem, Bucket bucket, PlotContent interactedPlot, Spawner interactedSpawner,
		Water interactedWater) {
		if (heldItem) {
			if (interactedPlot) {
				PlotContent plotcontent = interactedPlot.GetComponent<PlotContent>();
				plotcontent.PlantItem(heldItem);
				heldItem.gameobject.SetActive(false);
				heldItem = null;
			}
			else if (interactedWater) { }
		}
		else if (heldBucket) {
			if (interactedWater) {
				heldBucket.Interact(interactedWater);
			}
			else if (interactedPlot) {
				PlotContent plotcontent = interactedPlot.GetComponent<PlotContent>();
				heldBucket.Interact(plotcontent);
			}
		}
		else {
			if (interactedItem) {
				heldItem = interactedItem;
			}
			else if (interactedSpawner) {
				heldItem = interactedSpawner.GetItem();
			}
			else if (bucket) {
				heldBucket = bucket;
				heldBucket.transform.position = transform.position + Vector3.up * 0.5f;
				heldBucket.transform.parent = transform;
			}
		}

		if (heldItem) {
			heldItem.gameobject.transform.position = transform.position + Vector3.up * 0.5f;
			heldItem.gameobject.transform.parent = transform;
		}
	}

	//well i told you to stop
	private void _DropItem() {
		if (heldBucket != null) {
			heldBucket.transform.parent = null;
			heldBucket = null;
		}
		else if (heldItem != null) {
			heldItem.gameobject.transform.parent = null;
			heldItem = null;
		}
	}

	private Transform _GetClosest(Vector3 pos) {
		Transform closest = null;
		float minDistSq = interactDist * interactDist;

		foreach (Transform t in proximity) {
			float distSq = (pos - t.position).sqrMagnitude;

			if (distSq < minDistSq) {
				minDistSq = distSq;
				closest = t;
			}
		}

		return closest;
	}
}