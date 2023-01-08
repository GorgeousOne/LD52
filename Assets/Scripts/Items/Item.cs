using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id;
    public string name;
    public string description;
    public GameObject prefab;
    public GameObject gameobject = null;

    public void onCreation()
    {
        gameobject = (GameObject)Instantiate(prefab);
        gameobject.transform.parent = GameObject.Find("ItemHandler").transform;
    }
}
