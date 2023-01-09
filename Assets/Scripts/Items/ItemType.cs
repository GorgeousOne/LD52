using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemType : ScriptableObject
{
	public int id;
	public string name;
	public int price;
	[Range(0.01f, 1)] public float npcChance;
	public GameObject prefab;
	public float growingtime = 10.0f;
	public Sprite seedSprite;
	public Sprite baseSprite;
	public Sprite plantedSprite;
	public Sprite growingSprite;
	public Sprite finishedSprite;
	public Sprite witheredSprite;


	public Item OnCreation(Vector2 pos, bool isSeed = false) {
		GameObject itemDrop = Instantiate(prefab, GameObject.Find("ItemHandler").transform);
		Item item = itemDrop.GetComponent<Item>();
		item.SetType(this, isSeed);
		return item;
		// gameobject.transform.parent = GameObject.Find("ItemHandler").transform;
	}
}
