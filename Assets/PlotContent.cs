using System;
using UnityEngine;

public class PlotContent : MonoBehaviour {
	
	[SerializeField] private Sprite emptyPlot;

	private bool _isWatered;
	private Item _plantedItem;
	
	public bool IsWatered {
		get { return _isWatered; }
		set { _isWatered = value; }
	}

	public Item PlantedItem {
		get { return _plantedItem; }
	}
	
	private SpriteRenderer _icon;
		
	// Start is called before the first frame update
	void OnEnable() {
		_icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
		_icon.sprite = emptyPlot;
	}

	public void PlantItem(Item plantItem) {
		if (IsTilled()) {
			throw new InvalidOperationException("Plot already tilled with" + _plantedItem.name);
		}
		_plantedItem = plantItem;
		_icon.sprite = plantItem.gameobject.GetComponent<SpriteRenderer>().sprite;
	}
	
	public bool IsTilled() {
		return _plantedItem != null;
	}

	public Item Harvest() {
		if (!IsTilled()) {
			throw new InvalidOperationException("Plot not tilled yet");
		}
		return _plantedItem;
	}
}