using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyVan : MonoBehaviour
{
    [SerializeField] VanInventory inventory;

    public void OnButtonPressed()
    {
        PlayerStats.instance.Money += inventory.TotalValue;
        inventory.RespawnVan();
    }
}
