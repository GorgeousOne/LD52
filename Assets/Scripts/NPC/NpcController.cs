using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour {

	public Item wantedItem;
	private Bubble _bubble;
	
	private void OnEnable() {
		_bubble = transform.GetComponentInChildren<Bubble>();
	}

	public void ThinkItem(float time) {
		_bubble.gameObject.SetActive(true);
		_bubble.DisplayItem(wantedItem, true);
		StartCoroutine(_HideBubbleLater(time));
	}
	
	public void SayItem() {
		_bubble.gameObject.SetActive(true);
		_bubble.DisplayItem(wantedItem, true);
	}
	
	private IEnumerator _HideBubbleLater(float time) {
		yield return new WaitForSeconds(time);
		ShutUp();
	}
	
	public void ShutUp() {
		_bubble.gameObject.SetActive(false);
	}
}