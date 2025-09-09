using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elara.TileSystem;
using Elara.GameManagement;

namespace Elara.UI
{
    public class ActionCardUI : MonoBehaviour
    {
        [System.Serializable]
        public struct ActionCardInfo 
        {
            [SerializeField]
            public Sprite icon;
            public ActionManager.ActionTypes action;
            public int resourceCost;
            public ResourceManager.ResourceType resourceType;
        }
        [SerializeField]
        public ActionCardInfo[] actionCardInfos;
        
        public ActionManager.ActionTypes targetAction;
        public Image actionIcon;
        Button actionButton;

        public ActionPanel actionPanel;

        private void Awake()
        {
            IntializeActionCard();
            actionButton = GetComponent<Button>();
        }
        public void IntializeActionCard()
        {
            actionIcon.sprite = GetActionCardInfo().icon;
        }
        public ActionCardInfo GetActionCardInfo() 
        {
            ActionCardInfo acOut = actionCardInfos[0];
            foreach (ActionCardInfo aci in actionCardInfos) 
            {
                if (aci.action == targetAction)
                    acOut = aci;
            }
            return acOut;
        }
        public void OnActionClick() 
        {
            actionPanel.ReportActionCardUIClicked(targetAction);
        }
    }
}