using Elara.GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Elara.TileSystem
{
    public class SaplingTile : EnvironmentTile
    {
        private void Awake()
        {
            maxResourceStack = 1;
            SetTileValues(0);
        }
        public override void TileActionHandler(ActionManager.ActionTypes actionType)
        {
            base.TileActionHandler(actionType);
            switch (actionType)
            {
                case ActionManager.ActionTypes.Replenish:
                    Debug.Log("Replenishing sapling!");
                    ReplenishResource(1);
                    if (currentResourceStack >= maxResourceStack)
                        tileType = TileTypes.Forest;
                    break;
                default:
                    break;
            }
        }
    }
}