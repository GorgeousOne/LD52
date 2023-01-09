using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcController : Interactable {

	public Item wantedItem;
	public Item Soul;

    public UnityEvent<NpcController> OnDeath;
    public UnityEvent<NpcController> OnItemReceive;
	public UnityEvent<NpcController> OnTargerReach;
	public int queueIndex;
	
	private Bubble _bubble;
	private Vector2 _walkTarget;
	private Vector2 _walkStartPos;
	private float _walkStartTime = -1;
	private float _walkDuration;
	
	private void OnEnable() {

		interactableType = InteractableType.Person;
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
        SpriteRenderer renderer = _bubble.GetComponent<SpriteRenderer>();
		renderer.enabled = true;
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
		OnItemReceive.Invoke(this);
	}

	public void Kill()
	{
        //_bubble.gameObject.SetActive(false);
        Item i = Instantiate(Soul);
        i.onCreation();
		i.gameobject.transform.position = transform.position + Vector3.up * 0.5f;
        OnDeath.Invoke(this);

    }
    public bool Trade(Item i)
	{
		Debug.Log("Trading...");
		if(i.id == wantedItem.id)
		{
			Destroy(i);
			ReceiveItem();
			return true;
		}
		return false;
	}

	public void Walk(int queueIdx, Vector2 target, float duration) {
		queueIndex = queueIdx;
		_walkStartPos = transform.position;
		_walkTarget = target;
		_walkDuration = duration;
		_walkStartTime = Time.time;
	}
}