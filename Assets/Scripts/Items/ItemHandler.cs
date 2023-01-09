using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemHandler : MonoBehaviour {
	private List<Item> _items;
	
	public Item GetClosestItem(Vector3 pos, Item holding, ref float minDistSq) {
		_items = FindObjectsOfType<Item>().ToList();
		Item closest = null;

		if (holding != null) {
			_items.Remove(holding);
		}
		foreach (Item item in _items) {
			if (item.transform.gameObject.activeInHierarchy) {
				float distSq = (pos - item.transform.position).sqrMagnitude;

				if (distSq < minDistSq) {
					minDistSq = distSq;
					closest = item;
				}
			}
		}
		return closest;
	}
}