using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBase : MonoBehaviour
{
    [SerializeField] protected Image img;
    [SerializeField] TextMeshProUGUI priceText;

    protected PlayerStats playerStats;
    private int upgradePrice = 10;

    public int UpgradePrice { get => upgradePrice; set { upgradePrice = value; priceText.text = $"{upgradePrice}"; } }

    protected virtual void Start()
    {
        playerStats = PlayerStats.instance;
    }
    public void OnHover()
    {
        if (playerStats.Money >= UpgradePrice)
            img.color = Globals.greenTransColor;
        else
            img.color = Globals.redTransColor;
    }

    public void OnHoverEnd()
    {
        img.color = Color.clear;
    }

    public void OnClick()
    {
        if (playerStats.Money < UpgradePrice)
            return;

        Upgrade();
        OnHover();
    }

    protected virtual void Upgrade() {}
}
