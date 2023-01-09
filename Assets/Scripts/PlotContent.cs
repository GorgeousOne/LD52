using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotContent : Interactable {
	[SerializeField] private Sprite emptyPlot;
	[SerializeField] private Sprite needWater;

	private bool _isWatered;
	private ItemType _plantedItemType;
	private int _fertilized = 1;
	private double _growingsince;

	private AudioSource _audio;

	private enum _stage {
		Barren,
		Planted,
		Growing,
		Finished,
		Withered
	}

	private _stage _currentstage;

	public bool IsWatered {
		get { return _isWatered; }
		set { _isWatered = value; }
	}

	public ItemType PlantedItemType {
		get { return _plantedItemType; }
	}

	private SpriteRenderer _icon;
	private SpriteRenderer _wateredIcon;
	private SpriteRenderer _watereSplashIcon;
	private SpriteRenderer _fertilizedIcon;

	// Start is called before the first frame update
	void OnEnable() {
		interactableType = InteractableType.Plot;
		_currentstage = _stage.Barren;
		_icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
		_wateredIcon = transform.GetChild(1).GetComponent<SpriteRenderer>();
		_watereSplashIcon = transform.GetChild(2).GetComponent<SpriteRenderer>();
		_fertilizedIcon = transform.GetChild(3).GetComponent<SpriteRenderer>();
		_icon.sprite = emptyPlot;
		
		_audio = GetComponent<AudioSource>();
	}

	public bool PlantItem(ItemType plantItemType) {
		if (plantItemType.id == 0) {
			_fertilized += 1;
			return true;
		}
		else if (plantItemType.id == -1) {
			return false;
		}

		if (IsTilled()) {
			return false; //throw new InvalidOperationException("Plot already tilled with " + _plantedItem.name);
		}

		_plantedItemType = plantItemType;
		_icon.sprite = _plantedItemType.plantedSprite;
		_currentstage = _stage.Planted;
		_growingsince = Time.time;
		_audio.Play();
		return true;
	}

	public bool IsTilled() {
		return _plantedItemType != null;
	}

	public Item Harvest() {
		if (!IsTilled()) {
			throw new InvalidOperationException("Plot not tilled yet");
		}

		Item result;

		if (_currentstage == _stage.Finished) {
			result = _plantedItemType.OnCreation(transform.position);
			// SpriteRenderer rend = result.gameobject.GetComponent<SpriteRenderer>();
			// rend.sprite = result.baseSprite;
			// result.grown = true;
		}
		else {
			Destroy(_plantedItemType);
			result = null;
		}

		_fertilized = 1;
		_plantedItemType = null;
		_currentstage = _stage.Barren;
		_icon.sprite = emptyPlot;
		return result;
	}

	void Update() {
		_watereSplashIcon.enabled = _isWatered;
		_fertilizedIcon.enabled = _fertilized > 1;
		_wateredIcon.enabled = (!IsWatered && _fertilized == 1) &&
		                       (_currentstage == _stage.Planted || _currentstage == _stage.Growing);
		switch (_currentstage) {
			case _stage.Barren:
				break;
			case _stage.Planted:
				if (Time.time - _growingsince > PlantedItemType.growingtime / _fertilized) {
					if (IsWatered || _fertilized > 1) {
						IsWatered = false;
						_currentstage = _stage.Growing;
						_icon.sprite = PlantedItemType.growingSprite;
						_growingsince = Time.time;
					}
					else {
						_currentstage = _stage.Withered;
						_icon.sprite = PlantedItemType.witheredSprite;
					}
				}
				break;
			case _stage.Growing:
				if (Time.time - _growingsince > (PlantedItemType.growingtime * 1.1) / _fertilized) {
					if (IsWatered || _fertilized > 1) {
						IsWatered = false;
						_currentstage = _stage.Finished;
						_icon.sprite = PlantedItemType.finishedSprite;
						_growingsince = Time.time;
					}
					else {
						_currentstage = _stage.Withered;
						_icon.sprite = PlantedItemType.witheredSprite;
					}
				}
				break;
			case _stage.Finished:
				if (Time.time - _growingsince > PlantedItemType.growingtime * 2 && _fertilized == 1) {
					_currentstage = _stage.Finished;
					_icon.sprite = PlantedItemType.witheredSprite;
				}
				break;
			case _stage.Withered:
				break;
		}
	}

	public void SetWatered() {
		IsWatered = true;
	}
}