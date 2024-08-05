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
            case ItemType.Speed:  return 30;
            case ItemType.BulletDamage: return 30;
            case ItemType.BulletSpeed: return 30;
            case ItemType.Heart: return 30;
      
        } 
    }
}
