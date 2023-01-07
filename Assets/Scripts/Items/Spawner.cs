using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    [SerializeField] private Item itemType;
    
    // Start is called before the first frame update
    void Start(){

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
