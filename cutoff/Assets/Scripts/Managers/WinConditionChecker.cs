using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionChecker : MonoBehaviour
{
    public static WinConditionChecker Instance;
    
    float amountOfTiles = DEFINES.MAP_WIDTH * DEFINES.MAP_HEIGHT;

    float[] amountOfControlledTilesByPlayer;

    [SerializeField] float[] percentageToWinBasedOnNumOfPlayers;

    public void IncreaseAmountOfControlledTiles(int playerId)
    {
        amountOfControlledTilesByPlayer[playerId - 1] += 1;

        float percentageControlled = amountOfControlledTilesByPlayer[playerId - 1] / amountOfTiles;

        if (percentageControlled == percentageToWinBasedOnNumOfPlayers[DEFINES.NUM_OF_PLAYERS - 1])
            Debug.LogError("Player " + playerId + " has won!");
    }

    public void DecreaseAmountOfControlledTiles(int playerId)
    {
        amountOfControlledTilesByPlayer[playerId - 1] -= 1;
    }

    private void Awake()
    {
        Instance = this;
        amountOfControlledTilesByPlayer = new float[DEFINES.NUM_OF_PLAYERS];

        for (int i = 0; i < amountOfControlledTilesByPlayer.Length; i++)
        {
            amountOfControlledTilesByPlayer[i] = 0;
        }
    }
}