using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToolUpgrade : UpgradeBase
{
    [SerializeField] ChipCollector chipCollector;

    protected override void Start()
    {
        base.Start();
        UpgradePrice = 20;
    }

    protected override void Upgrade()
    {
        playerStats.Money -= UpgradePrice;
        UpgradePrice = (int)(UpgradePrice * 2.5f);
        chipCollector.UpgradeRange();
    }
}
