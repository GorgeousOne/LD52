using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerInteraction : MonoBehaviour {
	//dont look at this
	
	[Min(0)] public int balance = 650;
	[SerializeField] private float interactDist = 1.00f;
	[SerializeField] private SpawnerHandler spawnerHandler;
	[SerializeField] private ItemHandler itemHandler;
	[SerializeField] private PlotHandler plotHandler;
	[SerializeField] private WaterHandler waterHandler;
	[SerializeField] private PeopleHandler peopleHandler;
	[SerializeField] private Bucket bucketHandler;
	[SerializeField] private Scythe scytheHandler;
	public UnityEvent<int> OnBalanceChange;
	
    private Item _heldItem;
	private Tool _heldTool;
	private List<Transform> proximity = new();

	[SerializeField] private Transform indicator;
	Renderer script_renderer;

	void OnEnable() {
		AudioSource[] audios = GetComponents<AudioSource>();
		script_renderer = indicator.GetComponent<Renderer>();
		script_renderer.enabled = false;
	}

	// pls dont
	void Update() {
		float distancesqr = interactDist * interactDist;
		Bucket bucket = null;
        Scythe scythe = null;
		Tool tool = null;

		Vector2 pos = transform.position + .5f * Vector3.up;
		
        PlotContent interactedPlot = plotHandler.GetClosestPlot(pos, ref distancesqr);
		Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(pos, ref distancesqr);
		Item interactedItem = itemHandler.GetClosestItem(pos, _heldItem, ref distancesqr);
		Water interactedWater = waterHandler.GetClosestWater(pos, ref distancesqr);
		NpcController interactedPerson = peopleHandler.GetClosestBargainer(pos, ref distancesqr);

        if (_heldTool == null) {
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
			indicator.position = interactedItem.transform.position;
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
			indicator.position = interactedPerson.transform.position + .5f * Vector3.up;
		}
		else {
			newState = false;
		}
		script_renderer.enabled = newState;
	}
	private void _HandleE(Item interactedItem, Tool tool, PlotContent interactedPlot, Spawner interactedSpawner,
		Water interactedWater, NpcController interactedPerson) {
		if (_heldItem)
		{
			if (interactedPlot)
			{
				PlotContent plotcontent = interactedPlot.GetComponent<PlotContent>();
				if (plotcontent.PlantItem(_heldItem.itemType))
				{
					_heldItem.transform.parent = null;
					_heldItem.gameObject.SetActive(false);
					_heldItem = null;
				}
			}
			else if (interactedPerson)
			{
				if (interactedPerson.Trade(_heldItem, ref balance))
				{
                    Destroy(_heldItem.gameObject);
                    OnBalanceChange.Invoke(balance);
                    _heldItem = null;
				}
			}
			else if (interactedSpawner && interactedSpawner is SoulGrinder grinder) {
				_heldItem = grinder.GetItem(_heldItem);
			}
		}
		else if (_heldTool)
		{
			if (interactedWater)
			{
				_heldTool.Interact(interactedWater);
			}
			else if (interactedPlot)
			{
				_heldTool.Interact(interactedPlot);
			}
			else if (interactedPerson)
			{
				_heldTool.Interact(interactedPerson);
			}
		}
		else
		{
			if (interactedItem)
			{
				_heldItem = interactedItem;
			}
			else if (interactedSpawner)
			{
				_heldItem = interactedSpawner.GetItem(ref balance);
				OnBalanceChange.Invoke(balance);
			}
			else if (tool)
			{
				_heldTool = tool;
				_heldTool.transform.position = transform.position + Vector3.up * 0.5f;
				_heldTool.transform.parent = transform;
			}
			else if (interactedPerson)
			{
			}
		}
		if (_heldItem) {
			_heldItem.transform.position = transform.position + Vector3.up * 0.5f;
			_heldItem.transform.parent = transform;
		}
	}

	//well i told you to stop
	private void _DropItem() {
		if (_heldTool != null) {
			_heldTool.transform.parent = null;
			_heldTool = null;
		}
		else if (_heldItem != null) {
			_heldItem.transform.parent = null;
			_heldItem = null;
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