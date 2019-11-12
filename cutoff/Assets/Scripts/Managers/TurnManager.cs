using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    
    public int ActivePlayer { get; private set; }

    [HideInInspector] public PlayerMovement[] Players;

    public void TurnTaken()
    {
        ActivePlayer += 1;

        if (ActivePlayer == DEFINES.NUM_OF_PLAYERS)
            ActivePlayer = 0;

        for (int i = 0; i < Players.Length; i++)
        {
            if (i != ActivePlayer)
                Players[i].enabled = false;
            else
                Players[i].enabled = true;
        }
    }

    void ActivateFirstPlayer()
    {
        Players[0].enabled = true;
    }

    private void Awake()
    {
        Instance = this;

        Players = new PlayerMovement[DEFINES.NUM_OF_PLAYERS];
        ActivePlayer = 0;

        FindObjectOfType<MapGenerator>().OnMapGenerated += ActivateFirstPlayer;
    }
}
