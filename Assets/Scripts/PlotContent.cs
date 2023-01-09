using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotContent : Interactable {
	
	[SerializeField] private Sprite emptyPlot;
    [SerializeField] private Sprite needWater;
	
    private bool _isWatered;
	private Item _plantedItem;

	private int _fertilized = 1;

    private double _growingsince;
    private enum _stage
    {
        Barren,
        Planted,
        Growing,
        Ripe,
        Withered
    }
    
    private _stage _currentstage;

    public bool IsWatered {
		get { return _isWatered; }
		set { _isWatered = value; }
	}

	public Item PlantedItem {
		get { return _plantedItem; }
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
	}

	public bool PlantItem(Item plantItem) {
		if(plantItem.id == 0)
		{
			_fertilized += 1;
			return true;
		}else if(plantItem.id == -1)
		{
			return false;
		}
		if (IsTilled()) {
			return false;//throw new InvalidOperationException("Plot already tilled with " + _plantedItem.name);
		}
		_plantedItem = plantItem;
		_icon.sprite = _plantedItem.plantedSprite;
        _currentstage = _stage.Planted;
		_growingsince = Time.time;
		return true;
	}
	
	public bool IsTilled() {
		return _plantedItem != null;
	}

	public Item Harvest() {
		if (!IsTilled()) {
			throw new InvalidOperationException("Plot not tilled yet");
		}
		Item result;
        if (_currentstage == _stage.Ripe)
		{
			result = _plantedItem;
            SpriteRenderer rend = result.gameobject.GetComponent<SpriteRenderer>();
			rend.sprite = result.baseSprite;
			result.grown = true;
        }
		else
		{
			Destroy(_plantedItem);
			result = null;
		}
		_fertilized = 1;
        _plantedItem = null;
        _currentstage = _stage.Barren;
        _icon.sprite = emptyPlot;
        return result;
	}

    void Update()
    {
		_watereSplashIcon.enabled = _isWatered;
		_fertilizedIcon.enabled = _fertilized > 1;
        _wateredIcon.enabled = (!IsWatered && _fertilized == 1) && (_currentstage == _stage.Planted || _currentstage == _stage.Growing);
        switch (_currentstage)
		{
			case _stage.Barren:
				break;
			case _stage.Planted:
				if (Time.time - _growingsince > PlantedItem.growingtime/_fertilized)
				{
					if (IsWatered || _fertilized > 1)
					{
                        IsWatered = false;
						_currentstage = _stage.Growing;
						_icon.sprite = PlantedItem.growingSprite;
						_growingsince = Time.time;
					}
					else
					{
                        _currentstage = _stage.Withered;
						_icon.sprite = PlantedItem.witheredSprite;
					}
				}
				break;
			case _stage.Growing:
				if (Time.time - _growingsince > (PlantedItem.growingtime * 1.1)/_fertilized)
				{
					if (IsWatered || _fertilized > 1)
					{
                        IsWatered = false;
                        _currentstage = _stage.Ripe;
						_icon.sprite = PlantedItem.finishedSprite;
						_growingsince = Time.time;
					}
                    else
                    {
                        _currentstage = _stage.Withered;
                        _icon.sprite = PlantedItem.witheredSprite;
                    }
                }
				break;

			case _stage.Ripe:
				if (Time.time - _growingsince > PlantedItem.growingtime * 2 && _fertilized == 1)
				{
					_currentstage = _stage.Ripe;
					_icon.sprite = PlantedItem.witheredSprite;
				}
				break;

			case _stage.Withered:
					break;
		}
    }

	public void SetWatered()
	{
		IsWatered = true;
	}
}
