using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHandler : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI soldLabel;
	[SerializeField] private TextMeshProUGUI balanceLabel;
	[SerializeField] private Image moodEmoji;
	[Range(30, 180)][SerializeField] private float moodIntervalStart = 45;
	[Range(1, 60)][SerializeField] private float moodIntervalEnd = 20;
	[Range(60, 600)][SerializeField] private float moodSlopeTime = 180;

	[SerializeField] private List<Sprite> moods;

	[SerializeField] private GameObject deathScreen;
	[SerializeField] private TextMeshProUGUI deathMessage;

	private int _soldCount;
	
	private int _crowdMood;
	private bool _isBargaining;
	private float _lastMooDrop;

	private float _currentMoodInterval;
	private float _gameStart;
	
	private AudioSource _music;
	private AudioSource _deathAudio;

	private void OnEnable() {
		AudioSource[] audios = GetComponents<AudioSource>();
		_music = audios[0];
		_deathAudio = audios[1];

		PeopleHandler peopleHandler = FindObjectOfType<PeopleHandler>();
		peopleHandler.OnItemSell.AddListener(_IncreaseMood);
		peopleHandler.OnBargainBegin.AddListener(_StartMoodDecrease);
		peopleHandler.OnItemSell.AddListener(_EndMoodDecrease);
		FindObjectOfType<PlayerInteraction>().OnBalanceChange.AddListener(_UpdateText);
		FindObjectOfType<Scythe>().OnKill.AddListener(_DecreaseMood);
		
		_crowdMood = moods.Count - 1;
		_gameStart = Time.time;
		_currentMoodInterval = moodIntervalStart;
		
		_music = GetComponent<AudioSource>();
		_UpdateEmoji();
	}

	private void Update() {
		if (_isBargaining && Time.time > _lastMooDrop + _currentMoodInterval) {
			float progress = (Time.time - _gameStart) / moodSlopeTime;
			_currentMoodInterval = Mathf.Lerp(moodIntervalStart, moodIntervalEnd, progress);
			_DecreaseMood();
		}
	}

	private void _StartMoodDecrease() {
		Debug.Log("MOOD starts dropping " + _crowdMood);
		_isBargaining = true;
		_lastMooDrop = Time.time;
	}
	
	private void _EndMoodDecrease() {
		_isBargaining = false;
	}
	
	private void _IncreaseMood() {
		_soldCount += 1;
		soldLabel.text = _soldCount.ToString();
		
		_crowdMood = Math.Min(_crowdMood + 1, moods.Count - 1);
		_UpdateEmoji();
	}
	
	private void _DecreaseMood() {
		_crowdMood -= 1;
		_lastMooDrop += _currentMoodInterval;
		Debug.Log("MOOD " + _crowdMood);
		
		if (_crowdMood < 0) {
			_DeathScreen("Angry Crowd");
			return;
		}
		_UpdateEmoji();
	}
	
	private void _UpdateText(int newBalance) {
		balanceLabel.SetText(newBalance.ToString());

		if (newBalance < 0) {
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