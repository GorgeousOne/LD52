using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StatsHandler : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI soldLabel;
	[SerializeField] private TextMeshProUGUI balanceLabel;
	[SerializeField] private Image moodEmoji;
	[SerializeField] private float moodDropInterval = 20;

	[SerializeField] private List<Sprite> moods;

	[SerializeField] private GameObject deathScreen;
	[SerializeField] private TextMeshProUGUI deathMessage;

	private int _soldCount;
	
	private int _crowdMood;
	private float _lastMooDrop;
	
	private AudioSource _music;
	private AudioSource _deathAudio;

	private void OnEnable() {
		AudioSource[] audios = GetComponents<AudioSource>();
		_music = audios[0];
		_deathAudio = audios[1];

		FindObjectOfType<PeopleHandler>().OnItemSell.AddListener(IncreaseMood);
		FindObjectOfType<PlayerInteraction>().OnBalanceChange.AddListener(_UpdateText);
		FindObjectOfType<Scythe>().OnKill.AddListener(_DecreaseMood);
		
		_crowdMood = moods.Count - 1;
		_lastMooDrop = Time.time;
		_music = GetComponent<AudioSource>();
		_UpdateEmoji();
	}

	private void Update() {
		if (Time.time > _lastMooDrop + moodDropInterval) {
			_DecreaseMood();
		}
	}

	private void IncreaseMood() {
		_soldCount += 1;
		soldLabel.text = _soldCount.ToString();
		
		_crowdMood = Math.Min(_crowdMood + 1, moods.Count - 1);
		_lastMooDrop = Time.time;
		
		_UpdateEmoji();
	}
	
	private void _DecreaseMood() {
		_crowdMood -= 1;
		_lastMooDrop += moodDropInterval;
		Debug.Log("MOOD " + _crowdMood);
		
		if (_crowdMood < 0) {
			_DeathScreen("Angry Crowd");
			return;
		}
		_UpdateEmoji();
	}
	
	private void _UpdateText(int newBalance) {
		balanceLabel.SetText(newBalance.ToString());

		if (newBalance == 0) {
			_DeathScreen("Being Broke");
		}
	}

	private void _UpdateEmoji() {
		moodEmoji.sprite = moods[_crowdMood];
		_music.pitch = 1 + .1f * (moods.Count - 1 - _crowdMood);
	}

	private void _DeathScreen(string message) {
		Time.timeScale = 0;
		_music.Stop();
		_deathAudio.Play();
		deathScreen.SetActive(true);
		deathMessage.text = message;
	}
}