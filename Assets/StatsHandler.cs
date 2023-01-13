using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsHandler : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI soldLabel;
	[SerializeField] private TextMeshProUGUI balanceLabel;
	[SerializeField] private Slider moodBar;
	[Range(1, 180)][SerializeField] private float moodIntervalStart = 45;
	[Range(1, 60)][SerializeField] private float moodIntervalEnd = 20;
	[Range(60, 600)][SerializeField] private float moodSlopeTime = 180;

	[SerializeField] private List<Sprite> moods;

	[SerializeField] private GameObject deathScreen;
	[SerializeField] private TextMeshProUGUI deathMessage;

	private int _soldCount;
	private float _crowdMood;
	private bool _isBargaining;
	private float _gameStart;
	private Image _moodBarIcon;	
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

		_gameStart = Time.time;
		_music = GetComponent<AudioSource>();

		moodBar.maxValue = 1;
		moodBar.value = 1;
		_crowdMood = 1;
		_moodBarIcon = moodBar.handleRect.GetComponent<Image>();
		_UpdateEmoji();
	}

	private void Update() {

		if (_isBargaining) {
			float progress = (Time.time - _gameStart) / moodSlopeTime;
			float dropRate = 1 / Mathf.Lerp(moodIntervalStart, moodIntervalEnd, progress);
			float newMood = _crowdMood - Time.deltaTime * dropRate;
			_crowdMood = newMood;
		}
		moodBar.value = _crowdMood;
		_UpdateEmoji();

		if (_crowdMood < 0) {
			_DeathScreen("Angry Crowd");
		}
	}

	private void _StartMoodDecrease() {
		_isBargaining = true;
	}
	
	private void _EndMoodDecrease() {
		_isBargaining = false;
	}

	private void _DecreaseMood() {
		_crowdMood -= 1f / moods.Count;
		_UpdateEmoji();
	}
	
	private void _IncreaseMood() {
		_soldCount += 1;
		_crowdMood += 1f / moods.Count;
		soldLabel.text = _soldCount.ToString();
		_UpdateEmoji();
	}
	
	private void _UpdateText(int newBalance) {
		balanceLabel.SetText(newBalance.ToString());

		if (newBalance < 0) {
			_DeathScreen("Being Broke");
		}
	}

	private void _UpdateEmoji() {
		int moodIndex = Mathf.Clamp((int) (_crowdMood * moods.Count), 0, moods.Count - 1);
		_moodBarIcon.sprite = moods[moodIndex];
		_music.pitch = 1 + .1f * (moods.Count - 1 - (int) _crowdMood);
	}

	private void _DeathScreen(string message) {
		Time.timeScale = 0;
		_music.Stop();
		_deathAudio.Play();
		deathScreen.SetActive(true);
		deathMessage.text = message;
	}
}