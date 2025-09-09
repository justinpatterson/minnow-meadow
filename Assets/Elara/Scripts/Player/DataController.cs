using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Elara.GameManagement
{
    public class DataController : MonoBehaviour
    {
        ResourceManager rm;
        private void Start()
        {
            rm = GetComponent<ResourceManager>();
        }
        public void LoadGame()
        {

        }

        private void DataParser()
        {

        }
        public void SaveGame()
        {
            string myPath;

#if UNITY_ANDROID
            myPath = Application.persistentDataPath + "/auxillaryData.txt"; //Android
#elif UNITY_IOS
            myPath = Application.persistentDataPath +"/auxillaryData.txt"; //iOS
#else
            myPath = Application.persistentDataPath + "/auxillaryData.txt"; //PC
#endif

            StreamWriter sw = new StreamWriter(myPath, true);

            foreach(ResourceManager.ResourceType type in ResourceManager.ResourceType.GetValues(typeof(ResourceManager.ResourceType)))
            {
                sw.WriteLine(type.ToString() + rm.GetResourceCount(type).ToString());
            }

            //Do the grid state and tile information here
            /*
             * 
             */

            //Other information can go here

            sw.Close();
        }
    }
}
