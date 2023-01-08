using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class StatsHandler : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI balanceLabel;
	[SerializeField] private Image moodEmoji;

	[SerializeField] private Sprite moodGood;
	[SerializeField] private Sprite moodMe;
	[SerializeField] private Sprite moodBad;
	
	private void OnEnable() {
		Spawner[] spawners = FindObjectsOfType<Spawner>();
		foreach (Spawner spawner in spawners) {
			spawner.OnBalanceChange.AddListener(OnItemBuy);
		}
	}

	private void OnItemBuy(int newBalance) {
		balanceLabel.SetText(newBalance.ToString());
	}
}