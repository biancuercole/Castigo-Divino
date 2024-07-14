using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "ScriptableObjects/Loot")]

public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;

    public Loot (string lootName, int dropChance)
    {
        this.lootName = lootName;   
        this.dropChance = dropChance;
    }
}
