using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackUpgrade : UpgradeBase
{
    protected override void Start()
    {
        base.Start();
        UpgradePrice = 25;
    }

    protected override void Upgrade()
    {
        playerStats.Money -= UpgradePrice;
        UpgradePrice = (int)(UpgradePrice * 1.65f);
        playerStats.ChipInventorySize = (int)(playerStats.ChipInventorySize * 2f);
    }
}
