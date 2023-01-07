using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	[SerializeField] private Sprite thoughtShape;
	[SerializeField] private Sprite speechShape;

	private SpriteRenderer _renderer;

	private void OnEnable() {
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void DisplayItem(Item item, bool isThought) {
		_renderer.sprite = isThought ? thoughtShape : speechShape;
		
	}
}