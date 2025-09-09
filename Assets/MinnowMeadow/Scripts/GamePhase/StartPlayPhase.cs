using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elara.GameManagement {
    public class StartPlayPhase : GamePhaseBehavior
    {
        public override void StartPhase()
        {
            base.StartPhase();
            //GameManager.instance.GetUIManager().OpenGamePhasePanel(GameManager.GamePhases.Start, true);
        }
        public override void EndPhase()
        {
            base.EndPhase();
        }
    }
}