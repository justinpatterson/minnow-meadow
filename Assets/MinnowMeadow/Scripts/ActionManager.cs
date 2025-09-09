using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.TileSystem;
namespace Elara.GameManagement
{
    public class ActionManager : MonoBehaviour
    {
        
        TileAction _currentAction;
        public enum ActionTypes { Grow, Build, Hunt, Chop, Burn, Replenish, Drought, NewLife }
        public enum ActionAgent { Player, Environment, Event }
        public delegate void ActionComplete(ActionRequest actionRequest);
        public ActionComplete OnActionCompleted;

        [System.Serializable]
        public struct ActionRequest 
        {
            public float requestTime;
            public EnvironmentTile tile;
            public ActionTypes actionType;
            public ActionAgent actionAgent;
        }
        List<ActionRequest> currentActionRequestQueue = new List<ActionRequest>();
        Dictionary<int, List<ActionRequest>> turnActionRequestsMap = new Dictionary<int, List<ActionRequest>>();

        public List<ActionRequest> GetActionRequestsForCurrentTurn() 
        {
            if (turnActionRequestsMap.ContainsKey(GameplayPhase.roundNumber)) 
            {
                return turnActionRequestsMap[GameplayPhase.roundNumber];
            }
            else
                return new List<ActionRequest>();

        }
        private void Update()
        {
            if (currentActionRequestQueue.Count > 0) 
            {
                if (_currentAction == null) 
                {
                    PerformCurrentAction(currentActionRequestQueue[0]);
                }
            }
        }

        public void QueueAction(EnvironmentTile tileTarget, ActionTypes actionType, ActionAgent actionAgent) 
        {
            ActionRequest ar = new ActionRequest();
            ar.actionType = actionType;
            ar.tile = tileTarget;
            ar.actionAgent = actionAgent;
            currentActionRequestQueue.Add(ar);

            if (!turnActionRequestsMap.ContainsKey(GameplayPhase.roundNumber))
            {
                turnActionRequestsMap.Add(GameplayPhase.roundNumber, new List<ActionRequest>() { ar });
            }
            else 
            {
                turnActionRequestsMap[GameplayPhase.roundNumber].Add(ar);
            }
            //Debug.Log("Actions for this turn: " + turnActionRequestsMap[GameplayPhase.roundNumber].Count);
            
        }

        void PerformCurrentAction(ActionRequest actionRequest)
        {
            TileAction ta_inst = gameObject.AddComponent<TileAction>();
            _currentAction = ta_inst;
            ta_inst.DoAction(actionRequest, this);
        }

        public void ReportActionComplete(ActionRequest actionRequest) 
        {
            //ActionBehaviorPrefab would report back to ActionManager 
            if (OnActionCompleted != null)
                OnActionCompleted(actionRequest);

            //destroy that ActionBehaviorPrefab
            Destroy(_currentAction);
            currentActionRequestQueue.RemoveAt(0);
            _currentAction = null;
        }


        public bool ValidateActionRequest(ActionManager.ActionRequest actionRq)
        {
            switch (actionRq.tile.tileType)
            {
                case EnvironmentTile.TileTypes.Soil:
                    return (actionRq.actionType == ActionManager.ActionTypes.Build
                        || actionRq.actionType == ActionManager.ActionTypes.Grow
                        || actionRq.actionType == ActionManager.ActionTypes.NewLife);
                    //break;
                case EnvironmentTile.TileTypes.Village:
                    return (actionRq.actionType == ActionManager.ActionTypes.Burn);
                    //break;
                case EnvironmentTile.TileTypes.Forest:
                    return (actionRq.actionType == ActionManager.ActionTypes.Burn
                        || actionRq.actionType == ActionManager.ActionTypes.Replenish
                        || actionRq.actionType == ActionManager.ActionTypes.Chop);
                    //break;
                case EnvironmentTile.TileTypes.Ocean:
                    return (actionRq.actionType == ActionManager.ActionTypes.Hunt
                        || actionRq.actionType == ActionManager.ActionTypes.Drought
                        || actionRq.actionType == ActionManager.ActionTypes.Replenish
                        || actionRq.actionType == ActionManager.ActionTypes.NewLife);
                    //break;
            }
            return true;
        }
    }
}
