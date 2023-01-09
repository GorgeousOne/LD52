using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGrinder : Spawner
{
    public override Item GetItem(ref int balance) {
        return null;
    }
    
    public Item GetItem(Item soul)
    {
        if (soul.itemType.id == -1)
        {
            Destroy(soul.gameObject);
            return itemType.OnCreation(transform.position + .5f * Vector3.up);
        }
        return soul;
    }
    public override void OnEnable()
    {
    }


}
