using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Speed,
        BulletDamage,
        BulletSpeed,
        Heart
    }

    public static int GetCost(ItemType itemType) { 
        switch (itemType) {
        default:
            case ItemType.Speed:  return 1;
            case ItemType.BulletDamage: return 2;
            case ItemType.BulletSpeed: return 3;
            case ItemType.Heart: return 5;
      
        } 
    }
}
