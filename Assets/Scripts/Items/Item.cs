using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id;
    public string name;
    public string description;
    [SerializeField] private GameObject prefab;
    public GameObject gameobject = null;
    public Sprite baseSprite;
    public Sprite plantedSprite;
    public Sprite growingSprite;
    public Sprite finishedSprite;
    public Sprite witheredSprite;

    public float growingtime = 10.0f;

    public void onCreation()
    {
        gameobject = (GameObject)Instantiate(prefab);
        gameobject.transform.parent = GameObject.Find("ItemHandler").transform;
    }
}
