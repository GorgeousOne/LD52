using UnityEngine;

public class Item : MonoBehaviour {
	
	public ItemType itemType;
	private SpriteRenderer _icon;
	private bool _isSeed;
	public bool IsSeed => _isSeed;

	private void OnEnable() {
		_icon = GetComponent<SpriteRenderer>();

		if (itemType) {
			SetType(itemType);
		}
	}

	public void SetType(ItemType type, bool isSeed = false) {
		itemType = type;
		_isSeed = isSeed;
		_icon.sprite = _isSeed ? itemType.seedSprite : itemType.baseSprite;
	}
}