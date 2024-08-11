using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    List<Loot> GetDroppedItems()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if(randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            return possibleItems;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        List<Loot> droppedItems = GetDroppedItems();
        if (droppedItems != null)
        {
            foreach (Loot item in droppedItems)
            {
             
                Vector3 randomOffset = new Vector3(Random.Range(-2f, 1f), Random.Range(-2f, 1f), 0);
                GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition + randomOffset, Quaternion.identity);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = item.lootSprite;

                float dropForce = 300f;
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);

                if(item.lootName == "heart")
                {
                    lootGameObject.tag = "heart";
                }
                if(item.lootName == "coin")
                {
                    lootGameObject.tag = "coin";
                }
                if(item.lootName == "key")
                {
                    lootGameObject.tag = "key";
                }
            }
        }
    }
}

