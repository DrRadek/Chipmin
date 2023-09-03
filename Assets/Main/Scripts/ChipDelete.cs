using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

            if (chip.IsDestroyed())
                continue;

            Destroy(chip.gameObject);
        }
    }
}
