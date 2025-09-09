using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elara.UI;
using Elara.TileSystem;

namespace Elara.GameManagement
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        public delegate void GamePhaseTransitionEvent(GamePhases phase);
        public GamePhaseTransitionEvent OnGamePhaseTransition;

        UIManager _uiManager;
        public UIManager GetUIManager() 
        {
            if (_uiManager != null) return _uiManager;
            else
            {
                if (FindObjectOfType<UIManager>()) 
                {
                    _uiManager = FindObjectOfType<UIManager>();
                    return _uiManager;
                }
                if (UIManagerPrefab != null)
                {
                    GameObject prefabInst = Instantiate(UIManagerPrefab) as GameObject;
                    _uiManager = prefabInst.GetComponent<UIManager>();
                    return _uiManager;
                }
                else
                    return null;
            }
        }
        public GameObject UIManagerPrefab;

        TileManager _tileManager;
        public TileManager GetTileManager() 
        {
            if (_tileManager == null) {
                _tileManager = GameObject.FindObjectOfType<TileManager>();
                //jpatt - I don't think we should spawn it in. Might further complicate listeners
                    //altenrative - work it into a GameManager prefab so no outside refs are needed
            }
            return _tileManager;
        }

        public ResourceManager resourceManager = new ResourceManager();
        
        private void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
            }
            else
                Destroy(gameObject);
        }
        void Start()
        {
            /*
            forest = Instantiate(forestPrefab).GetComponent<Elara.TileSystem.ForestTile>();
            forest.SetTileValues(3, 0, 1);
            playerResources = new PlayerResources();
            */

            DoPhaseTransition(GamePhases.Initialize);
        }


        #region Phase Transition
        public enum GamePhases { Initialize, Start, Gameplay, End }
        public GamePhases currentPhase = GamePhases.Initialize;
        
        public GamePhaseBehavior[] phases;
        GamePhaseBehavior _lastPhaseBehavior;
        
        public void DoPhaseTransition(GamePhases targetPhase) 
        {

            if (_lastPhaseBehavior != null) _lastPhaseBehavior.EndPhase();

            currentPhase = targetPhase;
            _lastPhaseBehavior = GetPhaseBehavior(currentPhase);
            if (_lastPhaseBehavior != null)
            {
                //Debug.Log("Telling phase behavior to start " + _lastPhaseBehavior.name);
                _lastPhaseBehavior.StartPhase();
            }
            else { Debug.Log("No phase found for " + targetPhase.ToString()); }

            if (OnGamePhaseTransition != null)
                OnGamePhaseTransition(currentPhase);
        }
        GamePhaseBehavior GetPhaseBehavior(GamePhases targetPhase) 
        {
            GamePhaseBehavior returnBehavior = null;
            foreach (GamePhaseBehavior gpb in phases) 
            {
                if (gpb.phase == targetPhase)
                    returnBehavior = gpb; //jpatt: doing it this way because I vaguely recall foreach loops having an annoying memory quirk when returning complex vars
            }
            return returnBehavior;
        }

        #endregion



        public enum ResourceType
        {
            Wood, Fish
        };

        


        // Update is called once per frame
        void Update()
        {
            _lastPhaseBehavior?.UpdatePhase();
        }
    }
}