using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotContent : MonoBehaviour {
	
	[SerializeField] private Sprite emptyPlot;
    [SerializeField] private Sprite needWater;

    private bool _isWatered;
	private Item _plantedItem;

    private double _growingsince;
    private enum _stage
    {
        baron,
        planted,
        growing,
        finished,
        withered
    };
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
		
	// Start is called before the first frame update
	void OnEnable() {
		_currentstage = _stage.baron;
		_icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _wateredIcon = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _icon.sprite = emptyPlot;
	}

	public void PlantItem(Item plantItem) {
		if (IsTilled()) {
			throw new InvalidOperationException("Plot already tilled with " + _plantedItem.name);
		}
		_plantedItem = plantItem;
		_icon.sprite = _plantedItem.plantedSprite;
        _currentstage = _stage.planted;
		_growingsince = Time.time;
	}
	
	public bool IsTilled() {
		return _plantedItem != null;
	}

	public Item Harvest() {
		if (!IsTilled()) {
			throw new InvalidOperationException("Plot not tilled yet");
		}
		Item result = _plantedItem;
		_plantedItem = null;
		return result;
	}
    void Update()
    {
        _wateredIcon.enabled = !IsWatered && (_currentstage == _stage.planted || _currentstage == _stage.growing);
        switch (_currentstage)
		{
			case _stage.baron:
				break;
			case _stage.planted:
				{
					if (Time.time - _growingsince > PlantedItem.growingtime)
					{
						if (!IsWatered)
						{
                            IsWatered = false;
							_currentstage = _stage.growing;
							_icon.sprite = PlantedItem.growingSprite;
							_growingsince = Time.time;
						}
						else
						{
                            _currentstage = _stage.withered;
							_icon.sprite = PlantedItem.witheredSprite;
						}
					}
					break;
				}
			case _stage.growing:
				{
					if (Time.time - _growingsince > PlantedItem.growingtime * 1.1)
					{
						if (!IsWatered)
						{
                            IsWatered = false;
                            _currentstage = _stage.finished;
							_icon.sprite = PlantedItem.finishedSprite;
							_growingsince = Time.time;
						}
                        else
                        {
                            _currentstage = _stage.withered;
                            _icon.sprite = PlantedItem.witheredSprite;
                        }
                    }
					break;
				}

			case _stage.finished:
				{
					if (Time.time - _growingsince > PlantedItem.growingtime * 2)
					{
						_currentstage = _stage.finished;
						_icon.sprite = PlantedItem.witheredSprite;
					}
					break;

				}
			case _stage.withered:
				{
					break;
				}
		}
    }
	public void water(Bucket bucket)
	{
		if (bucket._isfilled)
		{
			IsWatered = true;
			bucket.empty();
        }
	}
}
