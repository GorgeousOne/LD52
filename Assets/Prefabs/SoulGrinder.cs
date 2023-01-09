using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGrinder : Spawner
{
    public override Item GetItem(ref int balance)
    {
        return null;
    }
    public Item GetItem(Item soul)
    {
        if (soul.id == -1)
        {
            Destroy(soul);
            Item gSoul = Instantiate(itemType);
            gSoul.onCreation();
            return gSoul;
        }
        return soul;
    }
    public override void OnEnable()
    {
    }


}
