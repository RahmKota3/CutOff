using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public static MapManager Instance;

    public Color[] PlayerColors;

    Tile[,] tiles;
    Dictionary<Tile, GameObject> tileToGameObject;

    public void SubmitMove(int x, int y, int playerId)
    {
        TurnManager.Instance.TurnTaken();

        ChangeTileOwner(x, y, playerId);

        CheckForIslands(playerId);
    }

    public void ChangeTileOwner(int x, int y, int tileOwner)
    {
        if(tiles[x, y].TileOwner != 0)
        {
            WinConditionChecker.Instance.DecreaseAmountOfControlledTiles(tiles[x, y].TileOwner);
        }
        WinConditionChecker.Instance.IncreaseAmountOfControlledTiles(tileOwner);

        tiles[x, y].TileOwner = tileOwner;

        UpdateTileGraphics(x, y);
    }

    public void SetTile(int x, int y, Tile t, GameObject g)
    {
        tiles[x, y] = t;
        tileToGameObject[t] = g;
    }

    void CheckForIslands(int playerId)
    {
        List<Tile> checkedTiles = new List<Tile>();
        List<Tile> tilesToChange = new List<Tile>();
        int numOfIslands = 0;
        int lastNumOfIslands = 0;

        for (int x = 0; x < DEFINES.MAP_WIDTH; x++)
        {
            for (int y = 0; y < DEFINES.MAP_HEIGHT; y++)
            {
                if (tiles[x, y].TileOwner == 0 
                    && x != 0 && x < DEFINES.MAP_WIDTH && y != 0 && y < DEFINES.MAP_HEIGHT)
                    numOfIslands += DFS(x, y, checkedTiles, playerId);

                if (lastNumOfIslands != numOfIslands)
                {
                    checkedTiles.Add(null);
                    lastNumOfIslands = numOfIslands;
                }
            }
        }
        
        if (numOfIslands > 1)
        {
            int i = 0;
            bool isSuitable = true;
            Dictionary<int, bool> islandsToChangeIndexes = new Dictionary<int, bool>();
            
            while (checkedTiles.Count > 0)
            {
                if (checkedTiles[0] == null)
                {
                    islandsToChangeIndexes[i] = isSuitable;
                    isSuitable = true;
                    i++;
                }
                else if (isSuitable)
                {
                    if (checkedTiles[0].x != playerId || checkedTiles[0].y == 0
                        || checkedTiles[0].x == DEFINES.MAP_WIDTH - 1 || checkedTiles[0].y == DEFINES.MAP_HEIGHT - 1 
                        /*|| NeighboursAreNeutralOrOwnedBy(checkedTiles[0], playerId) == false*/)
                    {
                        isSuitable = false;
                    }
                }

                if(checkedTiles.Count > 1)
                    tilesToChange.Add(checkedTiles[0]);
                
                checkedTiles.RemoveAt(0);
            }
                
            i = 0;

            foreach (Tile t in tilesToChange)
            {
                if (t == null)
                {
                    i++;
                }
                else
                {
                    if (islandsToChangeIndexes[i] == true)
                    {
                        ChangeTileOwner(t.x, t.y, playerId);
                    }
                }
            }
        }
    }

    int DFS(int x, int y, List<Tile> checkedTiles, int playerId)
    {
        if (x < 0 || y < 0 || x >= DEFINES.MAP_WIDTH || y >= DEFINES.MAP_HEIGHT || tiles[x, y].TileOwner == playerId
            || checkedTiles.Contains(tiles[x, y]))
            return 0;

        checkedTiles.Add(tiles[x, y]);
        DFS(x + 1, y, checkedTiles, playerId);
        DFS(x - 1, y, checkedTiles, playerId);
        DFS(x, y + 1, checkedTiles, playerId);
        DFS(x, y - 1, checkedTiles, playerId);

        DFS(x - 1, y - 1, checkedTiles, playerId);
        DFS(x + 1, y - 1, checkedTiles, playerId);
        DFS(x + 1, y + 1, checkedTiles, playerId);
        DFS(x - 1, y + 1, checkedTiles, playerId);

        return 1;
    }
    
    bool NeighboursAreNeutralOrOwnedBy(Tile tileToCheck, int owner)
    {
        if (tileToCheck.x < DEFINES.MAP_WIDTH - 1 && 
            tiles[tileToCheck.x + 1, tileToCheck.y].TileOwner != owner && tiles[tileToCheck.x + 1, tileToCheck.y].TileOwner != 0)
            return false;
        if (tileToCheck.x > 0 && 
            tiles[tileToCheck.x - 1, tileToCheck.y].TileOwner != owner && tiles[tileToCheck.x - 1, tileToCheck.y].TileOwner != 0)
            return false;
        if (tileToCheck.y < DEFINES.MAP_HEIGHT - 1 && 
            tiles[tileToCheck.x, tileToCheck.y + 1].TileOwner != owner && tiles[tileToCheck.x, tileToCheck.y + 1].TileOwner != 0)
            return false;
        if (tileToCheck.y > 0 &&
            tiles[tileToCheck.x, tileToCheck.y - 1].TileOwner != owner && tiles[tileToCheck.x, tileToCheck.y - 1].TileOwner != 0)
            return false;

        return true;
    }

    void UpdateTileGraphics(int x, int y) {
        tileToGameObject[tiles[x, y]].GetComponentInChildren<SpriteRenderer>().color = PlayerColors[tiles[x, y].TileOwner];
    }

    private void Awake() {
        Instance = this;

        tiles = new Tile[DEFINES.MAP_WIDTH, DEFINES.MAP_HEIGHT];
        tileToGameObject = new Dictionary<Tile, GameObject>();
    }
}
