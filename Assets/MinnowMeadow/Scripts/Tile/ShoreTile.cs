using Elara.GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Elara.TileSystem
{
    public class ShoreTile : EnvironmentTile
    {
        private void Awake()
        {
            SetTileValues(Random.Range(1, 4));
        }
        private void Start()
        {
            RefreshTileView();
        }

        public override bool IsExhausted()
        {
            return currentResourceStack == 0;
        }
        public override void TileActionHandler(ActionManager.ActionTypes actionType)
        {
            if (currentResourceStack > 0 && actionType == ActionManager.ActionTypes.Hunt) 
            {
                GameManager.instance.resourceManager.ResourceIncrement(ResourceManager.ResourceType.Fish, 3);
            }

            base.TileActionHandler(actionType);
            Debug.Log("Fish remaining: " + currentResourceStack + " out of " + maxResourceStack);

            RefreshTileView();
        }

        protected override void RefreshTileView()
        {
            base.RefreshTileView();
            GameObject instObj = GameManager.instance.GetTileManager().tileMap.GetInstantiatedObject(gridPos);
            Transform[] children = instObj.GetComponentsInChildren<Transform>();
            int i = 0;
            foreach (Transform t in instObj.transform)
            {
                t.gameObject.SetActive(i < currentResourceStack);
                i++;
            }
        }
    }
}