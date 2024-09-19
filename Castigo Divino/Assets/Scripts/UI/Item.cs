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
            case ItemType.Speed:  return 60;
            case ItemType.BulletDamage: return 90;
            case ItemType.BulletSpeed: return 60;
           // case ItemType.TripleShot: return 0;
      
        } 
    }


    public static Sprite GetSprite(ItemType itemType)
    {
        if (UIassets.i == null)
        {
            Debug.LogError("UIassets instance is null. Check if the prefab is correctly loaded.");
            return null;  // Retorna null o un sprite por defecto
        }

        switch (itemType)
        {
            default:
            case ItemType.Speed: return UIassets.i.Speed;
            case ItemType.BulletDamage: return UIassets.i.BulletDamage;
            case ItemType.BulletSpeed: return UIassets.i.BulletSpeed;
        }

    }
}
