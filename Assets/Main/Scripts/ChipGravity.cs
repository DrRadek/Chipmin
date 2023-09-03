using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipGravity : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (PlayerStats.instance.ToolMode != PlayerStats.ChipToolMode.In)
            return;

        if (!other.attachedRigidbody || !other.attachedRigidbody.TryGetComponent(out Chip _))
            return;

        other.attachedRigidbody.AddForce((transform.position - other.attachedRigidbody.transform.position).normalized * 70.0f, ForceMode.Acceleration);

        //Debug.DrawLine(transform.position, other.attachedRigidbody.transform.position);
    }
}
