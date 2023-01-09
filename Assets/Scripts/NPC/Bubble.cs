using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	[SerializeField] private Sprite thoughtShape;
	[SerializeField] private Sprite speechShape;

	private SpriteRenderer _renderer;

	private void OnEnable() {
		Thought t = GetComponentInChildren<Thought>();
        _renderer = t.gameObject.GetComponent<SpriteRenderer>();
    }

    public void DisplayItem(Item item, bool isThought) {
		Debug.Log(_renderer.sprite);
		_renderer.enabled = true;
		_renderer.sprite = item.baseSprite;
		
	}
}