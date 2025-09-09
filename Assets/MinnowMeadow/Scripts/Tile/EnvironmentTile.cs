using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.GameManagement;
using UnityEngine.Tilemaps;

namespace Elara.TileSystem
{
    public class EnvironmentTile : MonoBehaviour
    {
        public enum TileTypes { Soil, Village, Forest, Ocean, Sapling, Shore }
        public TileTypes tileType;
        public Vector3Int gridPos;

        protected int currentResourceStack;
        protected int maxResourceStack = 3;
        protected bool resourceExhausted = false;

        public virtual bool CanReplenish() { return currentResourceStack < maxResourceStack; }
        public virtual bool IsExhausted() { return false; /*Saplings start at 0, so they can't be 'exhausted' */ }
        //create the tile and set the values
        public void SetTileValues(int SourceStacks)
        {
            currentResourceStack = SourceStacks;
        }

        public virtual void SetTileSelected(bool isSelected) 
        {
        
        }

        public virtual ActionManager.ActionTypes[] GetAvailablePlayerActions() 
        {
            switch (tileType)
            {
                case TileTypes.Soil:
                    return new ActionManager.ActionTypes[] {
                        ActionManager.ActionTypes.Build,
                        ActionManager.ActionTypes.Grow
                    };
                case TileTypes.Village:
                    return new ActionManager.ActionTypes[] { };
                case TileTypes.Forest:
                    return new ActionManager.ActionTypes[] {
                        ActionManager.ActionTypes.Chop
                    };
                case TileTypes.Sapling:
                    return new ActionManager.ActionTypes[] { };
                case TileTypes.Shore:
                default:
                    return new ActionManager.ActionTypes[] {
                        ActionManager.ActionTypes.Hunt
                    };
            }
        }
        public virtual void TileActionHandler(ActionManager.ActionTypes actionType) 
        {
            switch (actionType)
            {
                case ActionManager.ActionTypes.Grow:
                    if (tileType == TileTypes.Soil)
                    {
                        tileType = TileTypes.Sapling;
                    }
                    break;
                case ActionManager.ActionTypes.Build:
                    if (tileType == TileTypes.Soil) 
                    {
                        tileType = TileTypes.Village;
                    }
                    break;
                case ActionManager.ActionTypes.Hunt:
                    UseResource(1);
                    break;
                case ActionManager.ActionTypes.Chop:
                    UseResource(1);
                    if (tileType == TileTypes.Forest)
                    {
                        tileType = TileTypes.Soil;
                    }
                    break;
                case ActionManager.ActionTypes.Burn:
                    UseResource(currentResourceStack);
                    break;
                case ActionManager.ActionTypes.Replenish:
                    if (!IsExhausted())
                    {
                        ReplenishResource(1);
                    }
                    break;
                case ActionManager.ActionTypes.Drought:
                    UseResource(currentResourceStack);
                    break;
                case ActionManager.ActionTypes.NewLife:
                    break;
            }
        }

        protected void ReplenishResource(int amount)
        {
            if (currentResourceStack < maxResourceStack)
            {
                //if the rate is larger than 1 we want to make sure to never pass max resources for a tile.
                currentResourceStack = currentResourceStack + amount > maxResourceStack ? maxResourceStack : currentResourceStack + amount;
            }
        }

        protected void UseResource(int amount)
        {
            currentResourceStack = Mathf.Clamp( currentResourceStack - amount, 0, maxResourceStack );
        }

        public int GetResourceStackAmount()
        {
            return currentResourceStack;
        }

        public int GetTurnsTilReplenishment()
        {
            return Mathf.Clamp((maxResourceStack - currentResourceStack), 0, maxResourceStack);
        }

        protected virtual void RefreshTileView() { }
    }
}