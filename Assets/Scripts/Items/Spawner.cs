using System;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour {
    
    protected AudioSource Audio;

    public ItemType itemType;
    public UnityEvent<int> OnBalanceChange;
    
    public virtual void OnEnable() {
        Audio = GetComponent<AudioSource>();
        try {
            SpriteRenderer icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
            icon.sprite = itemType.baseSprite;
        }
        catch (UnityException childoutofbounds) {}
    }

    public virtual Item GetItem(ref int balance) {//add a cost to the Item
        Item item = itemType.OnCreation(transform.position + Vector3.up, true);
        balance -= itemType.price;
        OnBalanceChange.Invoke(balance);
        Audio.Play();
        return item;
    }
}
