using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.TileSystem;
using UnityEngine.Tilemaps;
public class ScriptableTile : Tile
{
    [SerializeField]
    public Elara.TileSystem.EnvironmentTile.TileTypes type;

    public void ClickBehavior(Vector3Int position, Tilemap tileMap) 
    {
        Debug.Log("YOLO BABY");
        GameObject instObj = tileMap.GetInstantiatedObject(position);
    }
    
}
