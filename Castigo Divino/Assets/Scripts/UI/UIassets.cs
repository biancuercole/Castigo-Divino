using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIassets : MonoBehaviour
{
  private static UIassets _i;

  public static UIassets i
    {
        get
        {
            if (_i == null)
            {
                GameObject uiAssetsPrefab = Resources.Load("UIassets") as GameObject;
                if (uiAssetsPrefab == null)
                {
                    //Debug.LogError("UIassets prefab not found in Resources.");
                }
                else
                {
                    _i = Instantiate(uiAssetsPrefab).GetComponent<UIassets>();
                }
            }
            return _i;
        }
    }


    public Sprite Speed;
    public Sprite BulletDamage;
    public Sprite BulletSpeed; 
}
