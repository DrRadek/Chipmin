using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    PlayerStats playerStats;
    [SerializeField] PlayerController player;
    [SerializeField] SpeedUpgrade speedUpgrade;
    [SerializeField] BackpackUpgrade backpackUpgrade;
    [SerializeField] ChipCollector chipCollector;
    [SerializeField] ToolUpgrade toolUpgrade;

    [SerializeField] TMP_InputField inputField;

    [SerializeField] ChipDelete chipDelete;
    [SerializeField] PlayerController playerController;

    [SerializeField] List<FactoryBuilding> factoryBuildings = new();
    [SerializeField] List<FactoryMenu> factoryMenus = new();

    string saveName = "SaveName";

    public void UpdateSaveName()
    {
        saveName = inputField.text;
    }

    private void Start()
    {
        playerStats = PlayerStats.instance;
    }

    public void SaveEverything()
    {
        string path = GetSavePath();
        if (path == "")
            return;

        SaveData saveData = new()
        {
            money = playerStats.Money,
            
            maxGeneration = playerStats.MaxGeneration,
            newGenerationGoal = playerStats.newGenerationGoal,
            currentResearch = playerStats.CurrentResearch,

            chipInventoryCost = backpackUpgrade.UpgradePrice,
            chipInventorySize = playerStats.ChipInventorySize,

            speed = playerStats.Speed,
            speedCost = speedUpgrade.UpgradePrice,

            toolRangeCollector = chipCollector.collectorCollider.radius,
            toolRangeGravity = chipCollector.collectorCollider.radius,
            toolRangePrice = toolUpgrade.UpgradePrice
        };

        string json = JsonUtility.ToJson(saveData);
        
        System.IO.File.WriteAllText($"{path}/playerData.json", json);

        for(int i = 0; i < factoryBuildings.Count; i++)
        {
            FactoryBuilding building = factoryBuildings[i];
            FactoryMenu menu = factoryMenus[i];

            FactorySaveData data = new()
            {
                menuGeneration = menu.Generation,
                isBuilt = building.isBuilt,
                generation = building.generation,
                qualityUpgrade = building.qualityUpgrade,
                efficiencyUpgrade = building.efficiencyUpgrade,
                qualityUpgradePrice = building.qualityUpgradePrice,
                efficiencyUpgradePrice = building.efficiencyUpgradePrice
            };

            json = JsonUtility.ToJson(data);

            System.IO.File.WriteAllText($"{path}/Factory{i}.json", json);
        }
    }

    public void LoadEverything()
    {
        try
        {
            string path = GetSavePath();
            if (path == "")
                return;

            chipDelete.DeleteChips();
            playerController.RespawnPlayer();

            string json = System.IO.File.ReadAllText($"{path}/playerData.json");

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            playerStats.Money = saveData.money;

            playerStats.MaxGeneration = saveData.maxGeneration;
            playerStats.newGenerationGoal = saveData.newGenerationGoal;
            playerStats.CurrentResearch = saveData.currentResearch;

            backpackUpgrade.UpgradePrice = saveData.chipInventoryCost;
            playerStats.ChipInventorySize = saveData.chipInventorySize;

            playerStats.Speed = saveData.speed;
            speedUpgrade.UpgradePrice = saveData.speedCost;

            chipCollector.collectorCollider.radius = saveData.toolRangeCollector;
            chipCollector.gravityCollider.radius = saveData.toolRangeGravity;
            chipCollector.UpdateRangeText();
            toolUpgrade.UpgradePrice = saveData.toolRangePrice;

            for (int i = 0; i < factoryBuildings.Count; i++)
            {
                json = System.IO.File.ReadAllText($"{path}/Factory{i}.json");
                FactorySaveData data = JsonUtility.FromJson<FactorySaveData>(json);

                FactoryBuilding building = factoryBuildings[i];
                FactoryMenu menu = factoryMenus[i];

                if (!data.isBuilt)
                {
                    building.UnInitFactory();
                    continue;
                }
                    

                building.InitFactory(data.generation);

                building.qualityUpgrade = data.qualityUpgrade;
                building.efficiencyUpgrade = data.efficiencyUpgrade;
                building.qualityUpgradePrice = data.qualityUpgradePrice;
                building.efficiencyUpgradePrice = data.efficiencyUpgradePrice;

                menu.Generation = data.menuGeneration;
                menu.UpdateInfo();
            }
        }
        catch(Exception ex)
        {
            Debug.LogError($"Failed to load a save: {saveName}\n exception: {ex}");
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(0);
    }

    string GetSavePath(bool shouldExist = false)
    {
        var filePath = $"{Application.dataPath}/Saves/{saveName}";

        try
        {
            if (!Directory.Exists(filePath))
            {
                if (shouldExist)
                {
                    return "";
                }
                Directory.CreateDirectory(filePath);
            }

        }
        catch (IOException ex)
        {
            Debug.LogError(ex.Message);
            return "";
        }

        return filePath;
    }


    [Serializable]
    public class SaveData
    {
        public int money;

        public int maxGeneration;
        public int newGenerationGoal;
        public int currentResearch;

        public int speed;
        public int speedCost;

        public int chipInventorySize;
        public int chipInventoryCost;

        public float toolRangeCollector;
        public float toolRangeGravity;
        public int toolRangePrice;
    }

    public class FactorySaveData
    {
        public int menuGeneration;

        public bool isBuilt;
        public int generation;
        public int qualityUpgrade;
        public float efficiencyUpgrade;
        public int qualityUpgradePrice;
        public int efficiencyUpgradePrice;
    }
}
