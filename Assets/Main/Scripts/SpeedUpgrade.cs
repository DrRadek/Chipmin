using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpgrade : UpgradeBase
{
    protected override void Start()
    {
        base.Start();
        UpgradePrice = 10;
    }

    protected override void Upgrade()
    {
        playerStats.Money -= UpgradePrice;
        UpgradePrice = (int)(UpgradePrice * 1.4f);
        playerStats.Speed = (int)(playerStats.Speed * 1.1f);

    }
}
