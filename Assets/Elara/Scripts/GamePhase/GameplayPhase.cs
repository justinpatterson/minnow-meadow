using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.TileSystem;
using System;

namespace Elara.GameManagement {
    public class GameplayPhase : GamePhaseBehavior
    {
        public enum GameplaySubPhases
        {
            Initialize,
            Player,
            Environment,
            Event,
            Survival,
            End
        }

        public GameplaySubPhases currentGameplaySubphase = GameplaySubPhases.Initialize;

        //actions dicated by village count I think
        int maxActions = 2;
        int actionsRemaining = 0;

        ActionManager _actionManager;
        public static int roundNumber = 0;

        public override void StartPhase()
        {
            base.StartPhase();
            _actionManager = GameObject.FindObjectOfType<ActionManager>();
            SubPhaseTransition(GameplaySubPhases.Initialize);

        }
        public override void EndPhase()
        {
            base.EndPhase();
        }

        void SubPhaseTransition(GameplaySubPhases nextPhase)
        {
            EndSubPhase();
            currentGameplaySubphase = nextPhase;
            StartSubPhase();
        }

        public override void UpdatePhase()
        {
            base.UpdatePhase();
            UpdateSubPhase();
        }
        void StartSubPhase()
        {
            switch (currentGameplaySubphase)
            {
                case GameplaySubPhases.Initialize:
                    {
                        TileManager t = GameManager.instance.GetTileManager();
                        if (t != null)
                        {
                            Debug.Log("Initializing Tiles...");
                            t.InitializeTiles();
                            SubPhaseTransition(GameplaySubPhases.Player);
                        }
                        else { Debug.LogWarning("Could not find tile manager!"); }
                        break;
                    }
                case GameplaySubPhases.Player:
                    roundNumber++;
                    //maybe tell PlayerController to activate or whatever
                    //maybe inform GridManager it can start listening for clicks
                    actionsRemaining = maxActions;
                    _actionManager.OnActionCompleted += PlayerActionPerformed;
                    Debug.Log("Actions reset to ... " + actionsRemaining);
                    //listen for player clicks to be used for grid interaction
                    Elara.Player.PlayerController.OnScreenPosInteraction += PlayerScreenPosClickDelegate;
                    break;
                case GameplaySubPhases.Environment:
                    {
                        TileManager t = GameManager.instance.GetTileManager();
                        t.ReplenishTiles();
                        //TODO: WAIT FOR ACTIONS TO COMPLETE BEFORE GOING TO NEXT PHASE...
                        //SubPhaseTransition(GameplaySubPhases.Player);
                        break;
                    }
                case GameplaySubPhases.Event:
                    SubPhaseTransition(GameplaySubPhases.Survival);
                    break;
                case GameplaySubPhases.Survival:
                    SubPhaseTransition(GameplaySubPhases.Player);
                    break;
                case GameplaySubPhases.End:
                    break;
            }
        }

        private void PlayerScreenPosClickDelegate(Vector3 screenPos)
        {
            Debug.Log("Player Screen Position Handler Called");
            TileManager t = GameManager.instance.GetTileManager();
            if (t != null)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                worldPos.z = 0;
                t.PlayerInteractionAtWorldPosition(worldPos);
            }

        }

        private void PlayerActionPerformed(ActionManager.ActionRequest actionRequest)
        {
            actionsRemaining--;
            if (actionsRemaining <= 0)
            {
                SubPhaseTransition(GameplaySubPhases.Environment);
            }
            Debug.Log("Turns remaining: " + actionsRemaining);
        }

        public void ReportEnvironmentActionsPerformed() 
        {
            SubPhaseTransition(GameplaySubPhases.Event);
        }

        void UpdateSubPhase() 
        {
            switch (currentGameplaySubphase)
            {
                case GameplaySubPhases.Initialize:
                    break;
                case GameplaySubPhases.Player:
                    break;
                case GameplaySubPhases.Environment:
                    break;
                case GameplaySubPhases.Event:
                    break;
                case GameplaySubPhases.Survival:
                    break;
                case GameplaySubPhases.End:
                    break;
            }
        }
        void EndSubPhase() 
        {
            switch (currentGameplaySubphase)
            {
                case GameplaySubPhases.Initialize:
                    break;
                case GameplaySubPhases.Player:
                    //maybe tell PlayerController to DEactivate or whatever
                    //maybe inform GridManager it can STOP listening for clicks
                    _actionManager.OnActionCompleted -= PlayerActionPerformed; 
                    Elara.Player.PlayerController.OnScreenPosInteraction -= PlayerScreenPosClickDelegate;
                    break;
                case GameplaySubPhases.Environment:
                    break;
                case GameplaySubPhases.Event:
                    break;
                case GameplaySubPhases.Survival:
                    break;
                case GameplaySubPhases.End:
                    break;
            }
        }
    }
}