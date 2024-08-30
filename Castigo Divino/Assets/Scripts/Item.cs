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
        //TripleShot
    }

    public static int GetCost(ItemType itemType) { 
        switch (itemType) {
        default:
            case ItemType.Speed:  return 20;
            case ItemType.BulletDamage: return 50;
            case ItemType.BulletSpeed: return 30;
           // case ItemType.TripleShot: return 0;
      
        } 
    }
}
