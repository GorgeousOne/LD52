using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    [SerializeField] private Item itemType;

    private void OnEnable() {
        SpriteRenderer icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        icon.sprite = itemType.baseSprite;
    }

    // Update is called once per frame
    void Update(){
        
    }

    public Item GetItem() {//add a cost to the Item
        Item i = Instantiate(itemType);
        i.onCreation();
        return i;
    }
}
