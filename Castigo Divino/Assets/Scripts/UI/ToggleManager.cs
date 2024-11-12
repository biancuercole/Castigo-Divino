using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{

    public enum GameState
    {
        None,
        Dialogue,
        Shop,
        WeaponShop
    }

    public static GameState currentState = GameState.None;
}
