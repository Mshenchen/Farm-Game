using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
namespace Measy.Save 
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<ISaveable> saveableList = new List<ISaveable>();
        public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);
        private string jsonFolder;
        private int currentDataIndex;
        protected override void Awake()
        {
            base.Awake();
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                Save(currentDataIndex);
            if (Input.GetKeyDown(KeyCode.O))
                Load(currentDataIndex);
        }
        public void RegisterSaveable(ISaveable saveable)
        {
            if (!saveableList.Contains(saveable))
                saveableList.Add(saveable);
        }
        private void Save(int index)
        {
            DataSlot data = new DataSlot();

            foreach (var saveable in saveableList)
            {
                data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());
            }
            dataSlots[index] = data;

            var resultPath = jsonFolder + "data" + index + ".json";

            var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);

            if (!File.Exists(resultPath))
            {
                Directory.CreateDirectory(jsonFolder);
            }
            Debug.Log("DATA" + index + "SAVED!");
            File.WriteAllText(resultPath, jsonData);
        }
        public void Load(int index)
        {
            currentDataIndex = index;

            var resultPath = jsonFolder + "data" + index + ".json";

            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);

            foreach (var saveable in saveableList)
            {
                saveable.RestoreData(jsonData.dataDict[saveable.GUID]);
            }
        }
    }
}


