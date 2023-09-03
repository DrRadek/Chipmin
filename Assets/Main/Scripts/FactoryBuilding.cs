using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuilding : MonoBehaviour
{
    [SerializeField] GameObject BuiltMesh;
    [SerializeField] GameObject ToBeBuiltMesh;
    [SerializeField] Transform spawner;
    [SerializeField] Transform chipParent;


    [System.NonSerialized] public int generation = 0;
    [System.NonSerialized] public int qualityUpgrade = 0; // higher chip value
    [System.NonSerialized] public float efficiencyUpgrade = 1; // faster production
    [System.NonSerialized] public int qualityUpgradePrice = 0; // higher chip value
    [System.NonSerialized] public int efficiencyUpgradePrice = 0; // faster production
    public bool isBuilt = false;

    int chipPrice = 0;

    [SerializeField] Chip chipPrefab;

    //private void Start()
    //{
    //    //InitFactory();
    //}

    //public static NewFactoryBuildingInfo GetNewFactoryInfo(int generation)
    //{
    //    NewFactoryBuildingInfo info;
    //    info.cost = GetFactoryCost(generation);
    //    info.pricePerChip = GeChipPrice(generation);
    //    return info;
    //}

    public static int GetFactoryCost(int generation)
    {
        return (int)Mathf.Pow(2.0f, generation) * 142;
    }

    public static int GetChipPrice(int generation)
    {
        return (int)Mathf.Pow(2.1f, generation - 1);
    }

    public void InitFactory(int generation)
    {
        if (!isBuilt)
        {
            BuiltMesh.SetActive(true);
            ToBeBuiltMesh.SetActive(false);
        }

        qualityUpgradePrice = (int)((Mathf.Pow(1.9f, generation - 1) * 97)+1);
        efficiencyUpgradePrice = (int)((Mathf.Pow(1.9f, generation - 1)*21)+1);
        qualityUpgrade = 1;
        efficiencyUpgrade = 1;
        chipPrice = GetChipPrice(generation);
        isBuilt = true;

        this.generation = generation;
    }

    public void UnInitFactory()
    {
        if (isBuilt)
        {
            BuiltMesh.SetActive(false);
            ToBeBuiltMesh.SetActive(true);
        }

        isBuilt = false;
    }

    public int GetCurrentPrice()
    {
        return chipPrice * qualityUpgrade;
    }

    public void UpgradeQuality()
    {
        qualityUpgradePrice *= 2;
        qualityUpgrade += 1 + (int)(qualityUpgrade * 0.2f);
    }

    public void UpgradeEfficiency()
    {
        efficiencyUpgradePrice = (int)(efficiencyUpgradePrice * 1.5f);
        efficiencyUpgrade *= 0.8f;
    }

    public static int GetNewGenerationGoal(int generation)
    {
        return (int)Mathf.Pow(3.0f, generation + 1) * 25;
    }

    float timeSinceLastProduction = 0;

    private void Update()
    {
        if (!isBuilt)
            return;

        timeSinceLastProduction += Time.deltaTime;

        if (timeSinceLastProduction < efficiencyUpgrade)
            return;

        timeSinceLastProduction = 0;
        var chip = Instantiate(chipPrefab, chipParent);
        chip.price = GetCurrentPrice();
        chip.transform.SetPositionAndRotation(spawner.position,spawner.rotation);
        chip.rb.AddForce(spawner.forward * 22.0f, ForceMode.VelocityChange);
    }
}