using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlotContent : Interactable {
	[SerializeField] private Sprite emptyPlot;

	private bool _isWatered;
	private ItemType _plantedItemType;
	private bool _isFertilized;
	private double _growStartTime;

	private AudioSource _audio;

	private enum GrowStage {
		Barren,
		Planted,
		Growing,
		Finished,
		Withered
	}

	private GrowStage _currentstage;

	public bool IsWatered => _isWatered;

	public ItemType PlantedItemType {
		get { return _plantedItemType; }
	}

	private SpriteRenderer _icon;
	private SpriteRenderer _needsWaterIcon;
	private SpriteRenderer _waterSplashIcon;
	private SpriteRenderer _fertilizedIcon;

	// Start is called before the first frame update
	void OnEnable() {
		interactableType = InteractableType.Plot;
		_currentstage = GrowStage.Barren;
		_icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
		_needsWaterIcon = transform.GetChild(1).GetComponent<SpriteRenderer>();
		_waterSplashIcon = transform.GetChild(2).GetComponent<SpriteRenderer>();
		_fertilizedIcon = transform.GetChild(3).GetComponent<SpriteRenderer>();
		_icon.sprite = emptyPlot;
		
		_audio = GetComponent<AudioSource>();
	}

	public bool PlantItem(ItemType plantItemType) {
		if (plantItemType.id == 0) {
			if (_currentstage is GrowStage.Barren or GrowStage.Planted) {
				_isFertilized = true;
				_fertilizedIcon.enabled = true;
				return true;
			}
			return false;
		}
		if (plantItemType.id == -1) {
			return false;
		}
		if (IsTilled()) {
			return false; //throw new InvalidOperationException("Plot already tilled with " + _plantedItem.name);
		}
		if (!_isWatered) {
			_needsWaterIcon.enabled = true;
		}
		_plantedItemType = plantItemType;
		_icon.sprite = _plantedItemType.plantedSprite;
		_currentstage = GrowStage.Planted;
		_growStartTime = Time.time;
		_audio.Play();
		return true;
	}

	public bool IsTilled() {
		return _plantedItemType != null;
	}

	public void Harvest() {
		if (!IsTilled()) {
			throw new InvalidOperationException("Plot not tilled yet");
		}

		if (_currentstage == GrowStage.Finished) {
			for (int i = 0; i < (_isFertilized ? 2 : 1); ++i) {
				Vector3 rnd = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
				_plantedItemType.OnCreation(transform.position + .5f * rnd);
			}
		}
		else {
			Destroy(_plantedItemType);
		}
		_SetFertilized(false);
		SetWatered(false);
		_plantedItemType = null;
		_currentstage = GrowStage.Barren;
		_icon.sprite = emptyPlot;
	}

	void Update() {
		// _fertilizedIcon.enabled = _fertilized;
		// _wateredIcon.enabled = !IsWatered && (_currentstage == _stage.Planted || _currentstage == _stage.Growing);
		
		switch (_currentstage) {
			case GrowStage.Barren:
				break;
			case GrowStage.Planted:
				if (Time.time - _growStartTime > PlantedItemType.growingtime) {
					if (IsWatered) {
						_currentstage = GrowStage.Growing;
						_icon.sprite = PlantedItemType.growingSprite;
						_growStartTime += PlantedItemType.growingtime;
					}
					else {
						_currentstage = GrowStage.Withered;
						_icon.sprite = PlantedItemType.witheredSprite;
					}
					SetWatered(false);
				}
				break;
			case GrowStage.Growing:
				if (Time.time - _growStartTime > PlantedItemType.growingtime) {
					if (_isWatered) {
						_currentstage = GrowStage.Finished;
						_icon.sprite = PlantedItemType.finishedSprite;
						_growStartTime += PlantedItemType.growingtime;
					}
					else {
						_currentstage = GrowStage.Withered;
						_icon.sprite = PlantedItemType.witheredSprite;
					}
					SetWatered(false);
				}
				break;
			case GrowStage.Finished:
				if (Time.time - _growStartTime > PlantedItemType.growingtime * 3) {
					_currentstage = GrowStage.Withered;
					_icon.sprite = PlantedItemType.witheredSprite;
				}
				break;
			case GrowStage.Withered:
				_SetFertilized(false);
				break;
		}
	}
	
	public void SetWatered(bool state = true) {
		_isWatered = state;
		_waterSplashIcon.enabled = _isWatered;
		_needsWaterIcon.enabled = !_isWatered && _currentstage is GrowStage.Planted or GrowStage.Growing;
	}

	private void _SetFertilized(bool state = true) {
		_isFertilized = state;
		_fertilizedIcon.enabled = _isFertilized;
	}
} 