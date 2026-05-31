using UnityEngine;

public class PickupHint : MonoBehaviour
{
    public string message = "Press E to pick up";
    public float showDistance = 3f;

    private Transform player;
    private PickupSystem pickupSystem;
    private Outline outline;

    void Start()
    {
        player = Camera.main.transform;
        pickupSystem = FindFirstObjectByType<PickupSystem>();

        outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        bool canInteract =
            pickupSystem != null &&
            !pickupSystem.HasPackage() &&
            Vector3.Distance(player.position, transform.position) <= showDistance;

        if (canInteract)
        {
            UIManager.Instance.ShowHint(message);

            if (outline != null)
                outline.enabled = true;
        }
        else
        {
            UIManager.Instance.HideHint();

            if (outline != null)
                outline.enabled = false;
        }
    }
}