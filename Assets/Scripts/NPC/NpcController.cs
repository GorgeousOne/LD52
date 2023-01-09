using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class NpcController : Interactable {

	[FormerlySerializedAs("wantedItem")] public ItemType wantedItemType;
	public ItemType Soul;

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
		_bubble = transform.GetComponentInChildren<Bubble>(true);
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
		_bubble.DisplayItem(wantedItemType, true);
		StartCoroutine(_HideBubbleLater(time));
	}
	
	public void SayItem() {
		_bubble.gameObject.SetActive(true);
        _bubble.DisplayItem(wantedItemType, false);
	}
	
	private IEnumerator _HideBubbleLater(float time) {
		yield return new WaitForSeconds(time);
		ShutUp();
	}
	
	public void ShutUp() {
		_bubble.gameObject.SetActive(false);
	}
	
	public void ReceiveItem() {
	}

	public void Kill()
	{
        //_bubble.gameObject.SetActive(false);
        Item soulDrop = Soul.OnCreation(transform.position + 0.5f * Vector3.left);
        OnDeath.Invoke(this);
    }
	
    public bool Trade(Item i, ref int balance)
	{
		Debug.Log("Trading...");

		if (i.itemType.id != wantedItemType.id) {
			return false;
		}
		balance += i.itemType.price - 10;
		OnItemReceive.Invoke(this);
		return true;
	}

	public void Walk(int queueIdx, Vector2 target, float duration) {
		queueIndex = queueIdx;
		_walkStartPos = transform.position;
		_walkTarget = target;
		_walkDuration = duration;
		_walkStartTime = Time.time;
	}
}