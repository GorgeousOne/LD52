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

	[SerializeField] private GameObject deathScreen;
	[SerializeField] private TextMeshProUGUI deathMessage;
	
	private int _crowdMood;
	private float _lastMooDrop = -1;
	
	private AudioSource _music;
	private AudioSource _deathAudio;

	private void OnEnable() {
		AudioSource[] audios = GetComponents<AudioSource>();
		_music = audios[0];
		_deathAudio = audios[1];

		FindObjectOfType<PlayerInteraction>().OnBalanceChange.AddListener(_UpdateText);
		FindObjectOfType<Scythe>().OnKill.AddListener(DecreaseMood);
		_crowdMood = moods.Count - 1;
		_music = GetComponent<AudioSource>();
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

		if (newBalance == 0) {
			_DeathScreen("Being Broke");
		}
	}

	private void _UpdateEmoji() {
		moodEmoji.sprite = moods[_crowdMood];
		_music.pitch = 1 + .1f * (moods.Count - 1 - _crowdMood);
	}

	private void _DeathScreen(string message) {
		_music.Stop();
		_deathAudio.Play();
		deathScreen.SetActive(true);
		deathMessage.text = message;
	}
	
	public void DecreaseMood() {
		_crowdMood -= 1;
		_lastMooDrop = Time.time;

		if (_crowdMood < 0) {
			_DeathScreen("Angry Crowd");
			return;
		}
		_UpdateEmoji();
	}
}