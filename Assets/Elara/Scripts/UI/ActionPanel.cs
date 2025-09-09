using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.GameManagement;
using Elara.TileSystem;
namespace Elara.UI
{
    
    public class ActionPanel : UIPanel
    {
        //Dictionary<ActionManager.ActionTypes, ActionCardUI> _actionCardDictionary = new Dictionary<ActionManager.ActionTypes, ActionCardUI>();
        public ActionCardUI[] actionCards;

        public void LoadActions(ActionManager.ActionTypes[] inTypes) 
        {
            //I don't want to procedurally generate them because there's so few, and I'd rather pool them.
            //If we get more in the future, maybe it'll be procedural.
            
            List<ActionManager.ActionTypes> typesList = new List<ActionManager.ActionTypes>(inTypes);
            foreach (ActionCardUI ac in actionCards)
            {
                ac.gameObject.SetActive(typesList.Contains(ac.targetAction));
            }
        }

        public void ReportActionCardUIClicked(ActionManager.ActionTypes type) 
        {
            GameManager.instance.GetTileManager().ReportGridPlayerAction(type);
            //jpatt - should I close panel here?
            ClosePanel();
        }
    }

}
