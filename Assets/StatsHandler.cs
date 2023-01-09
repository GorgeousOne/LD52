using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHandler : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI balanceLabel;
	[SerializeField] private Image moodEmoji;
	[SerializeField] private float moodLiftInterval = 5;
	[SerializeField] private List<Sprite> moods;
	
	private int _crowdMood;
	private float _lastMooDrop = -1;
	
	private void OnEnable() {
		FindObjectOfType<PlayerInteraction>().OnBalanceChange.AddListener(_UpdateText);
		FindObjectOfType<Scythe>().OnKill.AddListener(DecreaseMood);
		_crowdMood = moods.Count - 1;
		_UpdateEmoji();
	}

	private void Update() {
		if (Time.time > _lastMooDrop + moodLiftInterval) {
			_lastMooDrop = Time.time;
			_crowdMood = Math.Min(_crowdMood + 1, moods.Count - 1);
			_UpdateEmoji();
		}
	}

	private void _UpdateText(int newBalance) {
		balanceLabel.SetText(newBalance.ToString());
	}

	private void _UpdateEmoji() {
		moodEmoji.sprite = moods[_crowdMood];
	}
	
	public void DecreaseMood() {
		_crowdMood -= 1;
		_lastMooDrop = Time.time;

		if (_crowdMood < 0) {
			Debug.Log("TOO MANY DIED!!!");
			return;
		}
		_UpdateEmoji();
	}
}