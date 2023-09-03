using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDelete : MonoBehaviour
{
    [SerializeField] Transform chipStorage;
    [SerializeField] List<VanInventory> vanInventories = new();


    public void DeleteChips()
    {
        foreach(Transform chip in chipStorage)
        {
            foreach (VanInventory inventory in vanInventories)
            {
                inventory.TryRemoveChip(chip.GetComponent<Chip>());
            }
            Destroy(chip.gameObject);
        }
    }
}
