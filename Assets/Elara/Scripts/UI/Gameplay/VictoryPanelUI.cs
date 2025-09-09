using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elara.UI
{
    public class VictoryPanelUI : UIPanel
    {
        public Sprite fillStar, emptyStar;
        public Image[] stars;
        private void Awake()
        {
            
        }
        private void Start()
        {
            RefreshVictoryPoints();
        }
        public void RefreshVictoryPoints() 
        {
            int victoryPointCount = GameManagement.GameManager.instance.resourceManager.GetResourceCount(GameManagement.ResourceManager.ResourceType.VictoryPoints);
            for (int i = 0; i < 10; i++) 
            {
                if (stars.Length > i) 
                {
                        stars[i].sprite = (i <= victoryPointCount) ? fillStar : emptyStar;
                }
            }
        }
    }
}