using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour {
    
    public ItemType itemType;
    public UnityEvent<int> OnBalanceChange;
    
    public virtual void OnEnable() {
        SpriteRenderer icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        icon.sprite = itemType.baseSprite;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public virtual Item GetItem(ref int balance) {//add a cost to the Item
        if (balance < itemType.price) {
            return null;
        }
        Item item = itemType.OnCreation(transform.position + Vector3.up, true);
        balance -= itemType.price;
        OnBalanceChange.Invoke(balance);
        return item;
    }
}
