using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FactoryMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI generationText;

    [SerializeField] TextMeshProUGUI qualityText;
    [SerializeField] TextMeshProUGUI efficiencyText;
    [SerializeField] TextMeshProUGUI chipPriceText;

    [SerializeField] TextMeshProUGUI incomeText;

    [SerializeField] TextMeshProUGUI priceText;

    [SerializeField] GameObject newBuilding;
    [SerializeField] GameObject ownedBuilding;

    [SerializeField] FactoryBuilding factory;

    [SerializeField] Image qualityImage;
    [SerializeField] Image efficiencyImage;

    [SerializeField] TextMeshProUGUI qualityPriceText;
    [SerializeField] TextMeshProUGUI efficiencyPriceText;

    int generation = 1;

    PlayerStats playerStats;

    int factoryCost = 0;

    public int Generation { get => generation; set => generation = value; }

    private void Start()
    {
        playerStats = PlayerStats.instance;

        UpdateInfo();
    }

    public void OnProductionQualityHover()
    {
        if (playerStats.Money >= factory.qualityUpgradePrice)
            qualityImage.color = Globals.greenTransColor;
        else
            qualityImage.color = Globals.redTransColor;
    }

    public void OnProductionQualityHoverEnd()
    {
        qualityImage.color = Color.clear;
    }

    public void OnProductionQualityClick()
    {
        if (playerStats.Money < factory.qualityUpgradePrice)
            return;

        playerStats.Money -= factory.qualityUpgradePrice;
        factory.UpgradeQuality();

        UpdateInfo();
        OnProductionQualityHover();
    }

    public void OnProductionEfficiencyHover()
    {
        if (playerStats.Money >= factory.efficiencyUpgradePrice)
            efficiencyImage.color = Globals.greenTransColor;
        else
            efficiencyImage.color = Globals.redTransColor;
    }

    public void OnProductionEfficiencyHoverEnd()
    {
        efficiencyImage.color = Color.clear;
    }

    public void OnProductionEfficiencyClick()
    {
        if (playerStats.Money < factory.efficiencyUpgradePrice)
            return;

        playerStats.Money -= factory.efficiencyUpgradePrice;
        factory.UpgradeEfficiency();

        UpdateInfo();
        OnProductionEfficiencyHover();
    }

    public void OnRightButtonPress()
    {
        if (Generation == playerStats.MaxGeneration)
            return;
        else
            Generation++;

        UpdateInfo();
    }

    public void OnLeftButtonPress()
    {
        if (Generation == 1)
            return;
        else
            Generation--;

        UpdateInfo();
    }

    public void OnBuyButtonPress()
    {
        if (factoryCost > playerStats.Money)
            return;

        playerStats.Money -= factoryCost;

        factory.InitFactory(Generation);

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        factoryCost = FactoryBuilding.GetFactoryCost(Generation);
        generationText.text = $"{Generation}";
        priceText.text = $"{factoryCost}";

        if (Generation == factory.generation)
        {
            qualityText.text = $"{factory.qualityUpgrade}";
            efficiencyText.text = $"{string.Format("{0:0.##}", 1 / factory.efficiencyUpgrade)}/s"; //$"{1/factory.efficiencyUpgrade}/s";
            chipPriceText.text = $"{factory.GetCurrentPrice()}";
            incomeText.text = $"{string.Format("{0:0.##}", factory.GetCurrentPrice() * (1 / factory.efficiencyUpgrade))}/s";

            newBuilding.SetActive(false);
            ownedBuilding.SetActive(true);

            qualityPriceText.text = $"{factory.qualityUpgradePrice}";
            efficiencyPriceText.text = $"{factory.efficiencyUpgradePrice}";
        }
        else
        {
            qualityText.text = "1";
            efficiencyText.text = $"1/s";
            chipPriceText.text = $"{FactoryBuilding.GetChipPrice(Generation)}";
            incomeText.text = $"{FactoryBuilding.GetChipPrice(Generation)}/s";

            newBuilding.SetActive(true);
            ownedBuilding.SetActive(false);
        }
    }

}
