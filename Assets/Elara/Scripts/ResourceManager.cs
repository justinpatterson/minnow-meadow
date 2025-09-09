using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elara.GameManagement
{
    public class ResourceManager
    {
        public enum ResourceType { Hearts, VictoryPoints, Wood, Fish }
        Dictionary<ResourceType, int> resourceInventory = new Dictionary<ResourceType, int>();

        public void InitializeResources()
        {
            resourceInventory.Clear();
            switch (Elara.Settings.Globals.DifficultyState)
            {
                case Settings.Globals.Difficulty.Easy:
                    resourceInventory.Add(ResourceType.Fish, 5);
                    resourceInventory.Add(ResourceType.Wood, 5);
                    resourceInventory.Add(ResourceType.VictoryPoints, 0);
                    resourceInventory.Add(ResourceType.Hearts, 5);
                    break;
                case Settings.Globals.Difficulty.Medium:
                    resourceInventory.Add(ResourceType.Fish, 4);
                    resourceInventory.Add(ResourceType.Wood, 4);
                    resourceInventory.Add(ResourceType.VictoryPoints, 0);
                    resourceInventory.Add(ResourceType.Hearts, 3);
                    break;
                case Settings.Globals.Difficulty.Hard:
                    resourceInventory.Add(ResourceType.Fish, 3);
                    resourceInventory.Add(ResourceType.Wood, 3);
                    resourceInventory.Add(ResourceType.VictoryPoints, 0);
                    resourceInventory.Add(ResourceType.Hearts, 3);

                    break;
            }
        }

        public delegate void ResourceUpdate(ResourceType resource, int total);
        public ResourceUpdate OnResourceUpdated;

        public void ResourceIncrement(ResourceType resource, int amount)
        {
            if (!resourceInventory.ContainsKey(resource))
                resourceInventory.Add(resource, 0);

            int total = resourceInventory[resource];
            total += amount;

            //jpatt - do any limits here if we want.
            resourceInventory[resource] = total;

            if (OnResourceUpdated != null)
                OnResourceUpdated(resource, total);
        }
        public int GetResourceCount(ResourceType resource) 
        {
            if ( !resourceInventory.ContainsKey(resource))
                resourceInventory.Add(resource, 0);
            
            return resourceInventory[resource];
        }
    }
}
