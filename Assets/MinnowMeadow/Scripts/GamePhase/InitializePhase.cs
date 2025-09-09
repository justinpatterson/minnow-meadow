using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elara.GameManagement {
    public class InitializePhase : GamePhaseBehavior
    {
        public override void StartPhase()
        {
            base.StartPhase();
            GameManager.instance.GetUIManager(); //will spawn the ui manager
            GameManager.instance?.DoPhaseTransition(GameManager.GamePhases.Start);
        }
        public override void EndPhase()
        {
            base.EndPhase();
        }
    }
}