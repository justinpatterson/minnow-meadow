using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.GameManagement;

namespace Elara.UI {
    public class UIManager : MonoBehaviour
    {
        private void Awake()
        {
            //jpatt note - 
            //there's a race condition for subscribing to game manager's phase transition.
            //I could technically make the GM wait before calling the initialize phase OR make the delegate "static" so it can be subscribed to on awake.
            GameManager.instance.GetUIManager().OpenGamePhasePanel(-1, true);
        }

        private void Start()
        {
            if (GameManager.instance)
            {
                GameManager.instance.OnGamePhaseTransition += UITransitionBehavior;
                GameManager.instance.GetUIManager().OpenGamePhasePanel(GameManager.instance.currentPhase, true);
            }
        }

        private void UITransitionBehavior(GameManager.GamePhases phase)
        {
            //Debug.Log("UI Transition is occurring...");
            OpenGamePhasePanel(phase, true);
        }

        [System.Serializable]
        struct MainPanel 
        {
            public string panelTitle;
            public GameManager.GamePhases panelPhase;
            public UIPanel panelTarget;
        }

        [SerializeField]
        MainPanel[] _mainPanels;
        [SerializeField]
        UIPanel _lastMainPanel;
        void OpenGamePhasePanel(int index, bool forceClose = false) 
        {
            //close prior panel(s)
            if (forceClose)
            {
                //Debug.Log("Closing all panels...");
                foreach (MainPanel mp in _mainPanels)
                    mp.panelTarget?.ClosePanel();
            }
            else
            {
                if (_lastMainPanel != null)
                    _lastMainPanel.ClosePanel();
            }

            //open new panel
            if (_mainPanels.Length > index && index >= 0) 
            {
                _lastMainPanel = _mainPanels[index].panelTarget;
                _lastMainPanel?.OpenPanel();
            }
            else 
            {
                _lastMainPanel = null;
                Debug.LogWarning("Attempting to open a non-index panel: " + index);
            }
        }
        public void OpenGamePhasePanel(GameManager.GamePhases inPhase, bool forceClose = false) 
        {
            for(int i = 0; i <  _mainPanels.Length; i++) 
            {
                if(_mainPanels[i].panelPhase == inPhase) 
                {
                    OpenGamePhasePanel(i, forceClose);
                    return;
                }
            }
            Debug.Log("No phase found, closing all.");
            OpenGamePhasePanel(-1, true);
        }
        public UIPanel GetMainPhasePanel(GameManager.GamePhases inPhase) 
        {
            for (int i = 0; i < _mainPanels.Length; i++)
            {
                if (_mainPanels[i].panelPhase == inPhase)
                {
                    return _mainPanels[i].panelTarget;
                }
            }
            return null;
        }
    }
}
