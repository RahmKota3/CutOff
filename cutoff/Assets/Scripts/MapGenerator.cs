using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject playerPrefab;

    Transform tileParent;

    public Action OnMapGenerated;

    void GenerateMap() {

        tileParent = new GameObject("TileParent").transform;
        tileParent.parent = this.transform;

        Tile t;
        GameObject g;

        for (int x = 0; x < DEFINES.MAP_WIDTH; x++) {
            for (int y = 0; y < DEFINES.MAP_HEIGHT; y++) {

                t = new Tile(x, y);

                g = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, tileParent);
                g.name = "Tile[" + x + ", " + y + "]";

                MapManager.Instance.SetTile(x, y, t, g);

            }
        }

        Camera.main.orthographicSize = Mathf.Max(DEFINES.MAP_HEIGHT, DEFINES.MAP_WIDTH) / 2 + 1;
        Camera.main.transform.position = new Vector3(DEFINES.MAP_WIDTH / 2, DEFINES.MAP_HEIGHT / 2 - 0.5f, -10);

        CreatePlayers();

        OnMapGenerated?.Invoke();
    }

    void CreatePlayers() {
        GameObject g;
        int x = 0;
        int y = 0;

        for (int i = 1; i < DEFINES.NUM_OF_PLAYERS + 1; i++) {
            
            switch (i)
            {
                case 2:
                    x = DEFINES.MAP_WIDTH - 1;
                    y = 0;
                    break;
                case 3:
                    x = DEFINES.MAP_WIDTH - 1;
                    y = DEFINES.MAP_HEIGHT - 1;
                    break;
                case 4:
                    x = 0;
                    y = DEFINES.MAP_HEIGHT - 1;
                    break;
            }

            g = Instantiate(playerPrefab, new Vector3(x, y), Quaternion.identity);
            g.GetComponent<SpriteRenderer>().color = MapManager.Instance.PlayerColors[i];
            g.GetComponent<PlayerStats>().Id = i;
            TurnManager.Instance.Players[i - 1] = g.GetComponent<PlayerMovement>();

            MapManager.Instance.ChangeTileOwner(x, y, i);
        }
    }

    private void Start() {
        GenerateMap();
    }
}
