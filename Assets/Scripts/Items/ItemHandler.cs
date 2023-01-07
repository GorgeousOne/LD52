using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemHandler : MonoBehaviour
{
    private List<Item> _items;

    // Start is called before the first frame update
    void Start()
    {
    }

    public Item GetClosestItem(Vector3 pos, float interactDist, Item holding)
    {
        _items = FindObjectsOfType<Item>().ToList();
        Item closest = null;
        if (holding != null)
            _items.Remove(holding);
        float minDistSq = interactDist * interactDist;

        foreach (Item item in _items)
        {
            float distSq = (pos - item.gameobject.transform.position).sqrMagnitude;

            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closest = item;
            }
        }
        return closest;
    }
}
