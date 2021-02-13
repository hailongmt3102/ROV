using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GenarateTileMap : MonoBehaviour
{
    // Start is called before the first frame update

    public string MapName;

    private Tile[] tiles;

    private Tilemap RealTileMap;
    private Tilemap VirtualMap;

    void Start()
    {
        RealTileMap =  transform.GetChild(0).GetComponent<Tilemap>();
        VirtualMap = transform.GetChild(1).GetComponent<Tilemap>();

        ReadMap();
        ReadTiles();
    }

    private void ReadMap() {
        // readmap from resource
        Image map = Resources.Load<Image>("Maps/Map 1/Levels" + MapName);
        if (map == null) {
            Debug.LogError("Can't load map image from path \"Maps/Map 1/Levels\"");
        }
    }

    private void ReadTiles() {
        tiles = Resources.LoadAll<Tile>("Maps/Map 1/Tile Palates/Tiles");
        if (tiles.Length == 0) {
            Debug.LogError("Can't load tiles from path \"Maps/Map1/Tile Palates/Tiles\"");
        }
    }

    private void GenerateMap() { 
    
    }
    


    // Update is called once per frame
}
