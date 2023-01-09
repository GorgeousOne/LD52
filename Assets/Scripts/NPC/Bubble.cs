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

    public void DisplayItem(ItemType itemType, bool isThought) {
		_renderer.enabled = true;
		_renderer.sprite = itemType.baseSprite;
	}
}