using Elara.GameManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Elara.TileSystem {
    public class TileManager : MonoBehaviour
    {
        public Tilemap tileMap;
        public Grid grid;


        [System.Serializable]
        public struct ScriptableTileMapping
        {
            public EnvironmentTile.TileTypes type;
            public TileBase scriptableTilePrefab;
        }
        public ScriptableTileMapping[] tileMappings;
        //Dictionary<Vector3Int, EnvironmentTile> tileMonoReference = new Dictionary<Vector3Int, EnvironmentTile>();

        Vector3Int _lastGridSelection = Vector3Int.zero;
        [SerializeField]
        int _envActionCnt = 0;
        
        private void Awake()
        {

            Debug.Log("CLEARING TILES...");
            tileMap.ClearAllTiles();

        }
        public void InitializeTiles(string defaultLayoutResourceName = "layout")
        {

            Debug.Log("CLEARING TILES...");
            tileMap.ClearAllTiles();
            TextAsset layout = (TextAsset) Resources.Load(defaultLayoutResourceName);
            string[] rows = layout.text.Split('\n');
            //Debug.Log("rows = " + rows.Length);
            int row = -1*(rows.Length/2);
            foreach (string r in rows) 
            {
                string[] columns = r.Split(',');
                int col = -1*(columns.Length/2);
                foreach (string c in columns) 
                {

                    //GameObject g = new GameObject(row + "_" + col);
                    //g.transform.SetParent(this.transform);
                    EnvironmentTile.TileTypes parsed_enum = (EnvironmentTile.TileTypes)System.Enum.Parse(typeof(EnvironmentTile.TileTypes), c);

                    Vector3Int v = new Vector3Int(col, row * -1, 0);
                    TileBase tileTarget = GetTileForType(parsed_enum);
                    tileMap.SetTile(v, tileTarget);
                    
                    GameObject instObj = tileMap.GetInstantiatedObject(v);
                    EnvironmentTile envTileInst = instObj.GetComponent<EnvironmentTile>();
                    if (envTileInst != null) 
                    {
                        envTileInst.tileType = parsed_enum;
                        envTileInst.gridPos = v;

                    }
                    col++;
                }
                row++;
            }
        }

        TileBase GetTileForType(EnvironmentTile.TileTypes t) 
        {
            foreach (ScriptableTileMapping stm in tileMappings) 
            { 
                if (stm.type == t) 
                    return stm.scriptableTilePrefab; 
            }
            return null;
        }

        public void PlayerInteractionAtWorldPosition(Vector3 worldPos) 
        {
            Vector3Int posInt = grid.LocalToCell(worldPos);

            // Shows the name of the tile at the specified coordinates
            ScriptableTile sTile = tileMap.GetTile(posInt) as ScriptableTile;
            if (sTile != null)
            {
                //TODO: maybe break away if Action Panel is already open.
                if (tileMap.GetInstantiatedObject(posInt) != null)
                {
                    //EXAMPLE OF AN ACTION... Should be performed down the line, not here:
                    //tileMonoReference[posInt].TileActionHandler(GameManagement.ActionManager.ActionTypes.Grow);

                    _lastGridSelection = posInt;
                    GameManagement.ActionManager.ActionTypes[] actions = GetEnvTileAtTilePosition(posInt).GetAvailablePlayerActions();
                    Elara.UI.UIManager um = GameManager.instance.GetUIManager();
                    Elara.UI.GameplayPanel gp = (Elara.UI.GameplayPanel)um.GetMainPhasePanel(GameManager.GamePhases.Gameplay);
                    gp.OpenActionPanel(actions);
                }
            }
        }

        EnvironmentTile GetEnvTileAtTilePosition(Vector3Int posInt) 
        {
            return tileMap.GetInstantiatedObject(posInt).GetComponent<EnvironmentTile>();
        }

        public void ReportGridPlayerAction(GameManagement.ActionManager.ActionTypes action) 
        {
            //tileMonoReference[_lastGridSelection].TileActionHandler(action);
            ActionManager am = GameObject.FindObjectOfType<ActionManager>();
            if (am != null) 
            {
                am.OnActionCompleted += PlayerActionCompleted;
                am.QueueAction(GetEnvTileAtTilePosition(_lastGridSelection), action, ActionManager.ActionAgent.Player);
            }

        }

        private void PlayerActionCompleted(ActionManager.ActionRequest actionRequest)
        {
            //check if tile visual should be updated
            EnvironmentTile currentEnvTile = GetEnvTileAtTilePosition(_lastGridSelection);
            EnvironmentTile.TileTypes type = currentEnvTile.tileType;
            TileBase tileTarget = GetTileForType(currentEnvTile.tileType);
            tileMap.SetTile(_lastGridSelection, tileTarget);

            if (currentEnvTile == null) //if the tilemap destroyed the old tile...
            {
                currentEnvTile = GetEnvTileAtTilePosition(_lastGridSelection);
                currentEnvTile.tileType = type;
                currentEnvTile.gridPos = _lastGridSelection;
            }
            
            //tileMonoReference[_lastGridSelection] = tileMonoReference[_lastGridSelection];

            //unsubscribe
            ActionManager am = GameObject.FindObjectOfType<ActionManager>();
            if (am != null)
            {
                am.OnActionCompleted -= PlayerActionCompleted;
            }
        }

        public void ReplenishTiles() 
        {
            ActionManager am = FindObjectOfType<ActionManager>();
            Debug.Log("Action Manager valid? " + (am != null));
            List<ActionManager.ActionRequest> ars = am.GetActionRequestsForCurrentTurn();
            List<Vector3Int> playerGridActions = new List<Vector3Int>();
            foreach (ActionManager.ActionRequest ar in ars) 
            {
                if(!playerGridActions.Contains(ar.tile.gridPos))
                    playerGridActions.Add(ar.tile.gridPos);
            }

            List<Vector3Int> EnvActionQueue = new List<Vector3Int>();

            for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++) 
            {
                for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++) 
                {
                    Vector3Int v = new Vector3Int(x, y, 0);
                    EnvironmentTile et = GetEnvTileAtTilePosition(v);
                    if (et.tileType == EnvironmentTile.TileTypes.Sapling)
                    {
                        Debug.Log("adding a sappling...");
                        EnvActionQueue.Add(v);
                    }
                    else if (et.tileType == EnvironmentTile.TileTypes.Forest || et.tileType == EnvironmentTile.TileTypes.Shore)
                    {
                        if(et.CanReplenish() /* && !et.IsExhausted()*/)
                            EnvActionQueue.Add(v);
                    }
                }
            }
            _envActionCnt = EnvActionQueue.Count;

            if (_envActionCnt > 0)
            {
                Debug.Log("Will perform actions on " + _envActionCnt + " objects.");
                am.OnActionCompleted += EnvironmentActionCompleted;
                foreach (Vector3Int v in EnvActionQueue)
                {
                    EnvironmentTile et = GetEnvTileAtTilePosition(v);
                    if (!playerGridActions.Contains(et.gridPos))
                    {
                        am.QueueAction(et, ActionManager.ActionTypes.Replenish, ActionManager.ActionAgent.Environment);
                    }
                }
            }
            else
            {
                _envActionCnt = 0;
                GameObject.FindObjectOfType<GameplayPhase>().ReportEnvironmentActionsPerformed();
            }
        }
        private void EnvironmentActionCompleted(ActionManager.ActionRequest actionRequest)
        {
            Debug.Log("Environment action complete!");
            //check if tile visual should be updated
            EnvironmentTile currentEnvTile = GetEnvTileAtTilePosition(actionRequest.tile.gridPos);
            EnvironmentTile.TileTypes type = currentEnvTile.tileType;
            TileBase tileTarget = GetTileForType(currentEnvTile.tileType);
            tileMap.SetTile(actionRequest.tile.gridPos, tileTarget);

            if (currentEnvTile == null) //if the tilemap destroyed the old tile...
            {
                currentEnvTile = GetEnvTileAtTilePosition(actionRequest.tile.gridPos);
                currentEnvTile.tileType = type;
                currentEnvTile.gridPos = actionRequest.tile.gridPos;
            }

            //tileMonoReference[_lastGridSelection] = tileMonoReference[_lastGridSelection];

            //unsubscribe logic
            _envActionCnt--;
            if (_envActionCnt <= 0)
            {
                ActionManager am = GameObject.FindObjectOfType<ActionManager>();
                if (am != null)
                {
                    am.OnActionCompleted -= EnvironmentActionCompleted;
                }

            }

            GameObject.FindObjectOfType<GameplayPhase>().ReportEnvironmentActionsPerformed();
        }
    }
}
