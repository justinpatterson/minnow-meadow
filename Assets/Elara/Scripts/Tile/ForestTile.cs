/* Created 8/25/2022
 * A forest tile has a constant replenish rate and takes X turns
 * to return to the full growth state that allows harvesting.
 * 
 * Harvesting a tree yields X wood that we pass to the player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.GameManagement;
using UnityEngine.Tilemaps;

namespace Elara.TileSystem
{
    public class ForestTile : EnvironmentTile
    {
       //Enum that defines resources for the game
        GameManager.ResourceType resource;

        public override void TileActionHandler(ActionManager.ActionTypes actionType)
        {
            base.TileActionHandler(actionType);
            switch (actionType)
            {
                case ActionManager.ActionTypes.Chop:
                    //remove 1 tree
                    GameManager.instance.resourceManager.ResourceIncrement(ResourceManager.ResourceType.Wood,
                        (Elara.Settings.Globals.DifficultyState == Settings.Globals.Difficulty.Easy) ? 3 : 2
                    );
                    //refresh tree view to look like "stump"
                    break;
                case ActionManager.ActionTypes.Burn:
                    //play fire anim
                    break;
                case ActionManager.ActionTypes.Replenish:
                    break;
                case ActionManager.ActionTypes.Grow:
                    break;
                default:
                    break;
            }
        }
        public override bool IsExhausted()
        {
            return currentResourceStack == 0;
        }

    }
}