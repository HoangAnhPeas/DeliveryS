using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PickupSystem pickup =
            other.GetComponent<PickupSystem>();

        if (pickup != null && pickup.HasPackage())
        {
            pickup.DeliverPackage();

            GameManager.Instance.CompleteDelivery();

            SpawnManager.Instance.SpawnNewDelivery();

            AudioManager.Instance.PlaySFX(
                AudioManager.Instance.deliveryClip
            );
        }
    }
}