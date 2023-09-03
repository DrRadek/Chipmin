using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchVan : MonoBehaviour
{
    [SerializeField] VanInventory inventory;

    public void OnButtonPressed()
    {
        PlayerStats.instance.CurrentResearch += inventory.TotalValue;
        inventory.RespawnVan();
    }
}
