using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcController : MonoBehaviour {

	public Item wantedItem;
	public UnityEvent<NpcController> OnItemReceive;
	public UnityEvent<NpcController> OnTargerReach;
	public int queueIndex;
	
	private Bubble _bubble;
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
			float progress = (Time.time - _walkStartTime) / _walkDuration;
			//Debug.Log("walk " + progress + "%");
			if (progress < 1) {
				transform.position = Vector2.Lerp(_walkStartPos, _walkTarget, progress);
			} else {
				transform.position = _walkTarget;
				_walkStartTime = -1;
				OnTargerReach.Invoke(this);
			}
		}
	}

	public void ThinkItem(float time) {
		_bubble.gameObject.SetActive(true);
		_bubble.DisplayItem(wantedItem, true);
		StartCoroutine(_HideBubbleLater(time));
	}
	
	public void SayItem() {
		Debug.Log("(cant say stuff yet)");
		// _bubble.gameObject.SetActive(true);
		// _bubble.DisplayItem(wantedItem, true);
	}
	
	private IEnumerator _HideBubbleLater(float time) {
		yield return new WaitForSeconds(time);
		ShutUp();
	}
	
	public void ShutUp() {
		_bubble.gameObject.SetActive(false);
	}
	
	public void ReceiveItem() {
		OnItemReceive.Invoke(this);
	}

	public void Kill()
	{
		_bubble.gameObject.SetActive(false);
		//Spawn Soul
		Destroy(this);
	}

	public void Walk(int queueIdx, Vector2 target, float duration) {
		queueIndex = queueIdx;
		_walkStartPos = transform.position;
		_walkTarget = target;
		_walkDuration = duration;
		_walkStartTime = Time.time;
	}
}