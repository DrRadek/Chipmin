using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class VanInventory : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button button;

    [SerializeField] Rigidbody parentRb;
    [SerializeField] Rigidbody doorsRb;
    [SerializeField] GameObject flyDirection;

    Vector3 respawnPos;
    Quaternion respawnRot;

    Vector3 respawnDoorPos;
    Quaternion respawnDoorRot;

    readonly HashSet<Chip> chips = new();
    int totalValue = 0;
    bool isRespawning = false;

    public int TotalValue { get => totalValue; private set { totalValue = value; text.text = $"{totalValue}"; } }

    private void Start()
    {
        respawnPos = parentRb.transform.position;
        respawnRot = parentRb.transform.rotation;

        respawnDoorPos = doorsRb.transform.position;
        respawnDoorRot = doorsRb.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (isRespawning)
        {
            parentRb.AddForce(flyDirection.transform.forward*10,ForceMode.Acceleration);
            parentRb.AddTorque(-parentRb.transform.right*100, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null || !other.attachedRigidbody.TryGetComponent(out Chip chip))
            return;

        chips.Add(chip);
        TotalValue += chip.price;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null || !other.attachedRigidbody.TryGetComponent(out Chip chip) || isRespawning)
            return;

        chips.Remove(chip);
        TotalValue -= chip.price;
    }

    public void DeleteChips()
    {
        foreach (Chip chip in chips)
        {
            Destroy(chip.gameObject);
        }

        chips.Clear();
        TotalValue = 0;
    }

    public void TryRemoveChip(Chip chip)
    {
        if (!chips.Contains(chip))
            return;

        chips.Remove(chip);
        TotalValue -= chip.price;
    }

    public void DisableChips()
    {
        foreach (Chip chip in chips)
        {
            chip.isReal = false;
        }
    }

    public void RespawnVan()
    {
        StartCoroutine(VanRespawn());
    }

    private IEnumerator VanRespawn()
    {
        button.gameObject.SetActive(false);
        DisableChips();
        isRespawning = true;
        yield return new WaitForSeconds(5);

        isRespawning = false;
        button.gameObject.SetActive(true);

        parentRb.velocity = Vector3.zero;
        parentRb.angularVelocity = Vector3.zero;

        parentRb.transform.SetPositionAndRotation(respawnPos, respawnRot);

        DeleteChips();

        doorsRb.velocity = Vector3.zero;
        doorsRb.angularVelocity = Vector3.zero;
        //doorsRb.AddTorque(Vector3.forward * 100, ForceMode.VelocityChange);

        doorsRb.transform.SetPositionAndRotation(respawnDoorPos, respawnDoorRot);

        //doorsRb.transform.eulerAngles = new Vector3(17, 0, 0);
    }

}
