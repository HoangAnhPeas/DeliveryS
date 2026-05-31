using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform holdPoint;

    [Header("Settings")]
    public float pickupRange = 3f;

    [HideInInspector]
    public GameObject heldPackage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldPackage == null)
            {
                TryPickup();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2)
        );

        RaycastHit hit;

        Debug.DrawRay(
            ray.origin,
            ray.direction * pickupRange,
            Color.red,
            2f
        );

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Package"))
            {
                Pickup(hit.collider.gameObject);
            }
        }
    }

    void Pickup(GameObject package)
    {
        heldPackage = package;

        Rigidbody rb = package.GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        package.transform.SetParent(holdPoint);

        package.transform.localPosition = Vector3.zero;
        package.transform.localRotation = Quaternion.identity;

        PickupHint hint = package.GetComponent<PickupHint>();
        if (hint != null)
            hint.SendMessage("Hide", SendMessageOptions.DontRequireReceiver);

        AudioManager.Instance.PlaySFX(
            AudioManager.Instance.pickupClip
        );
    }

    public void DeliverPackage()
    {
        Destroy(heldPackage);
        heldPackage = null;
    }

    public bool HasPackage()
    {
        return heldPackage != null;
    }
}