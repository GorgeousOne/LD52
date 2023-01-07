using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcController : MonoBehaviour {

	public Item wantedItem;
	private Bubble _bubble;
	public UnityEvent OnItemReceive;
	
	
	private Vector2 _walkTarget;
	private Vector2 _walkStartPos;
	private float _walkStartTime = -1;
	private float _walkDuration;
	
	private void OnEnable() {
		_bubble = transform.GetComponentInChildren<Bubble>();
	}

	private void Update() {
		if (_walkStartTime != -1) {
			float walkEndTime = _walkStartTime + _walkDuration;
			float progress = (walkEndTime - Time.time) / _walkDuration;

			if (walkEndTime < 1) {
				transform.position = Vector2.Lerp(_walkStartPos, _walkTarget, progress);
			} else {
				transform.position = _walkTarget;
				_walkStartTime = -1;
			}
		}
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
	
	public void ReceiveItem() {
		OnItemReceive.Invoke();
	}

	public void Walk(Vector2 target, float duration) {
		_walkStartPos = transform.position;
		_walkTarget = target;
		_walkDuration = duration;
		_walkStartTime = Time.time;
	}
}