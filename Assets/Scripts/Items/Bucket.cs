using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Tool {
	
	public bool _isfilled;
	[SerializeField] private Sprite _spriteFilled;
	[SerializeField] private Sprite _spriteEmpty;

	private SpriteRenderer _icon;

	void OnEnable() {
		toolType = ToolType.Bucket;
		_icon = transform.GetComponent<SpriteRenderer>();
		_icon.sprite = _spriteEmpty;
	}

	public override void Interact(Interactable i) {
		switch (i.interactableType) {
			case InteractableType.Water:
				_Fill();
				break;
			case InteractableType.Plot:
				PlotContent plot = i.GetComponent<PlotContent>();
				
				if (_isfilled && !plot.IsWatered) {
					plot.SetWatered();
					Empty();
				}
				break;
		}
	}

	private void _Fill()
	{
		_isfilled = true;
		_icon.sprite = _spriteFilled;
	}
	
	public void Empty()
	{
		_isfilled = false;
		_icon.sprite = _spriteEmpty;
	}
	
	public Bucket distancesqr(Vector3 pos, ref float sqrdis)
	{
		float bucketdistance = (pos - transform.position).sqrMagnitude;
		if (bucketdistance < sqrdis)
		{
			sqrdis = bucketdistance;
			return this;
		}
		return null;

	}
}
