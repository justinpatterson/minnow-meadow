using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Elara.GameManagement
{
    public class GamePhaseBehavior : MonoBehaviour
    {
        public GameManager.GamePhases phase;
        public virtual void StartPhase() { }
        public virtual void UpdatePhase() { }
        public virtual void EndPhase() { }
    }
}
