using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	//dont look at this
	[Min(0)] public int balance = 500;
	[SerializeField] private float interactDist = 1.00f;
	[SerializeField] private SpawnerHandler spawnerHandler;
	[SerializeField] private ItemHandler itemHandler;
	[SerializeField] private PlotHandler plotHandler;
	[SerializeField] private WaterHandler waterHandler;
	[SerializeField] private PeopleHandler peopleHandler;
	[SerializeField] private Bucket bucketHandler;
	[SerializeField] private Scythe scytheHandler;

    public Item heldItem;
	public Tool heldTool;
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
        Scythe scythe = null;
		Tool tool = null;

		Vector2 pos = transform.position;
		
        PlotContent interactedPlot = plotHandler.GetClosestPlot(pos, ref distancesqr);
		Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(pos, ref distancesqr);
		Item interactedItem = itemHandler.GetClosestItem(pos, heldItem, ref distancesqr);
		Water interactedWater = waterHandler.GetClosestWater(pos, ref distancesqr);
		NpcController interactedPerson = peopleHandler.GetClosestBargainer(pos, ref distancesqr);

        if (heldTool == null) {
			bucket = (Bucket)bucketHandler.distancesqr(pos, ref distancesqr);
			scythe = (Scythe)scytheHandler.distancesqr(pos, ref distancesqr);

			if (scythe)
				tool = scythe;
			else if (bucket)
				tool = bucket;
        }

		_ToggleIndicator(interactedItem, tool , interactedPlot, interactedSpawner, interactedWater, interactedPerson);

		if (Input.GetKeyUp(KeyCode.E)) {
			_HandleE(interactedItem, tool, interactedPlot, interactedSpawner, interactedWater, interactedPerson);
		}
		else if (Input.GetKeyUp(KeyCode.Q)) {
			_DropItem();
		}
	}

	private void _ToggleIndicator(Item interactedItem, Tool tool, PlotContent interactedPlot,
		Spawner interactedSpawner, Water interactedWater, NpcController interactedPerson) {
		bool newState = true;
		
		if (interactedItem != null) {
			indicator.position = interactedItem.gameobject.transform.position;
		}
		else if (tool) {
			indicator.position = tool.transform.position;
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
		}else if (interactedPerson) {
			indicator.position = interactedPerson.transform.position;
		}
		else {
			newState = false;
		}
		script_renderer.enabled = newState;
	}
	private void _HandleE(Item interactedItem, Tool tool, PlotContent interactedPlot, Spawner interactedSpawner,
		Water interactedWater, NpcController interactedPerson) {
		if (heldItem)
		{
			if (interactedPlot)
			{
				PlotContent plotcontent = interactedPlot.GetComponent<PlotContent>();
				if (plotcontent.PlantItem(heldItem))
				{
					heldItem.gameobject.transform.parent = null;
					heldItem.gameobject.SetActive(false);
					heldItem = null;
				}
			}
			else if (interactedWater) { }
			else if (interactedPerson)
			{
				interactedPerson.Trade();
			}
			else if (interactedSpawner)
			{
				if(interactedSpawner.itemType.id == 0)
					heldItem = ((SoulGrinder)interactedSpawner).GetItem(heldItem);
			}
		}
		else if (heldTool)
		{
			if (interactedWater)
			{
				heldTool.Interact(interactedWater);
			}
			else if (interactedPlot)
			{
				heldTool.Interact(interactedPlot);
			}
			else if (interactedPerson)
			{
				heldTool.Interact(interactedPerson);
			}
		}
		else
		{
			if (interactedItem)
			{
				heldItem = interactedItem;
			}
			else if (interactedSpawner)
			{
				heldItem = interactedSpawner.GetItem(ref balance);
			}
			else if (tool)
			{
				heldTool = tool;
				heldTool.transform.position = transform.position + Vector3.up * 0.5f;
				heldTool.transform.parent = transform;
			}
			else if (interactedPerson)
			{
			}
		}
		if (heldItem) {
			heldItem.gameobject.transform.position = transform.position + Vector3.up * 0.5f;
			heldItem.gameobject.transform.parent = transform;
		}
	}

	//well i told you to stop
	private void _DropItem() {
		if (heldTool != null) {
			heldTool.transform.parent = null;
			heldTool = null;
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