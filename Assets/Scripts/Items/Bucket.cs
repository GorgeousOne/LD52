using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Tool {
	
	public bool isFilled;
	[SerializeField] private Sprite _spriteEmpty;
	[SerializeField] private Sprite _spriteFilled;

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
				Debug.Log("water plot" + isFilled + !plot.IsWatered);
				
				if (isFilled && !plot.IsWatered) {
					Debug.Log("mhhh  yes");
					plot.SetWatered();
					Pour();
				}
				break;
		}
	}

	private void _Fill()
	{
		isFilled = true;
		_icon.sprite = _spriteFilled;
	}
	
	public void Pour()
	{
		isFilled = false;
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
