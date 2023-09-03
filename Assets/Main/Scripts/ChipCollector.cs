using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ChipCollector : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rangeText; 

    public SphereCollider collectorCollider;
    public SphereCollider gravityCollider;

    [SerializeField] GameObject collectorOutput;

    PlayerStats playerStats;

    readonly LinkedList<Chip> chips = new();
    [SerializeField] List<VanInventory> vanInventories = new();

    //readonly float outSpeed = 0.01f;
    //float outDelta = 0;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        rangeText.text = $"{gravityCollider.radius * 4}";
    }

    private void FixedUpdate()
    {
        if (PlayerStats.instance.ToolMode != PlayerStats.ChipToolMode.Out)
            return;

        //outDelta += Time.deltaTime;

        //if (outDelta < outSpeed)
        //    return;

        //outDelta = 0;

        if (chips.Count == 0)
            return;

        Chip chip = chips.First.Value;
        chips.RemoveFirst();
        playerStats.Chips--;

        chip.gameObject.SetActive(true);
        chip.transform.position = collectorOutput.transform.position;
        chip.rb.velocity = Vector3.zero;
        chip.rb.AddForce(collectorOutput.transform.forward * 100, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerStats.instance.ToolMode != PlayerStats.ChipToolMode.In || playerStats.Chips == playerStats.ChipInventorySize)
            return;

        if (!other.attachedRigidbody || !other.attachedRigidbody.TryGetComponent(out Chip chip) || !chip.isReal || chip.IsDestroyed())
            return;

        chips.AddLast(chip);
        playerStats.Chips++;

        chip.gameObject.SetActive(false);

        foreach(VanInventory inventory in vanInventories)
        {
            inventory.TryRemoveChip(chip);
        }
    }

    public void UpgradeRange()
    {
        collectorCollider.radius *= 1.1f;
        gravityCollider.radius *= 1.2f;
        UpdateRangeText();
    }

    public void UpdateRangeText()
    {
        rangeText.text = $"{gravityCollider.radius * 4}";
    }
}
