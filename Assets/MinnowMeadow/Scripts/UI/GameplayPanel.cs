using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.GameManagement;
using UnityEngine.UI;
using System;

namespace Elara.UI
{
    public class GameplayPanel : UIPanel
    {
        [SerializeField]
        ActionPanel actionPanel;
        [SerializeField]
        VictoryPanelUI victoryPanel;

        public Text fishCounter;
        public Text woodCounter;

        public override void OpenPanel()
        {
            actionPanel.ClosePanel();
            base.OpenPanel();
            victoryPanel.RefreshVictoryPoints();

            GameManager.instance.resourceManager.OnResourceUpdated += ResourceUpdateListener;
        }


        public void OpenActionPanel(ActionManager.ActionTypes[] actions) 
        {
            actionPanel.OpenPanel();
            actionPanel.LoadActions(actions);
        }

        void RefreshResourceCounters() 
        {
            fishCounter.text = GameManager.instance.resourceManager.GetResourceCount(ResourceManager.ResourceType.Fish).ToString("00");
            woodCounter.text = GameManager.instance.resourceManager.GetResourceCount(ResourceManager.ResourceType.Wood).ToString("00");
        }

        private void ResourceUpdateListener(ResourceManager.ResourceType resource, int total)
        {
            //jpatt - eh we could be more specific, but it's fine for now.
            RefreshResourceCounters();
        }
    }
}