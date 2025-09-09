using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Elara.TileSystem;
using Elara.GameManagement;
namespace Elara.Player
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void PlayerScreenPositionInteraction(Vector3 screenPos);
        public static PlayerScreenPositionInteraction OnScreenPosInteraction;

        public bool playerActive = true;
        private void Awake()
        {
#if UNITY_EDITOR
            //lol unity
            OnScreenPosInteraction = null;
#endif
        }
        private void Update()
        {
            if (!playerActive)
                return;

            if (Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                Vector3 screenPos = Input.mousePosition;
                screenPos.z = 0;
                if(OnScreenPosInteraction != null) OnScreenPosInteraction(screenPos);
            }
        }
        
    }
}