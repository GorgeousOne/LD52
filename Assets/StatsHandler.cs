using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UIElements.Image;

public class StatsHandler : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI balanceLabel;
	[SerializeField] private Image moodEmoji;

	[SerializeField] private Sprite moodGood;
	[SerializeField] private Sprite moodMeh;
	[SerializeField] private Sprite moodBad;

	private void OnEnable() {
		FindObjectOfType<PlayerInteraction>().OnBalanceChange.AddListener(UpdateText);
	}

	public void UpdateText(int newBalance) {
		balanceLabel.SetText(newBalance.ToString());

	}
}