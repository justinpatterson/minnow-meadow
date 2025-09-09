using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elara.UI
{
    public class StartPanel : UIPanel
    {
        public Button startGameButton;
        private void Awake()
        {
            startGameButton?.onClick.AddListener(() => OnStartClicked() );
        }
        void OnStartClicked() 
        {
            if (GameManagement.GameManager.instance != null)
                GameManagement.GameManager.instance.DoPhaseTransition(GameManagement.GameManager.GamePhases.Gameplay);
        }
    }
}
