using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionWaypoint : MonoBehaviour
{
    public Image img;
    public TMP_Text meter;

    public Vector3 offset;
    public float border = 50f;

    public Transform player;

    private Transform target;

    void Update()
    {
        UpdateTarget();

        if (target == null)
        {
            img.enabled = false;
            meter.enabled = false;
            return;
        }

        img.enabled = true;
        meter.enabled = true;

        Camera cam = Camera.main;

        Vector3 screenPos =
            cam.WorldToScreenPoint(target.position + offset);

        Vector3 dir =
            (target.position - player.position).normalized;

        bool behind =
            Vector3.Dot(player.forward, dir) < 0;

        float minX = border;
        float maxX = Screen.width - border;

        float minY = border;
        float maxY = Screen.height - border;

        // If the target is behind the player, we want to show the waypoint on the edge of the screen
        if (behind)
        {
            // Invert the screen position to get the opposite direction
            screenPos *= -1f;

            Vector2 screenCenter =
                new Vector2(Screen.width, Screen.height) / 2f;

            Vector2 fromCenter =
                ((Vector2)screenPos - screenCenter).normalized;

            // Calculate the scale factor to move the waypoint to the edge of the screen
            float scaleX =
                (fromCenter.x > 0)
                ? (maxX - screenCenter.x) / fromCenter.x
                : (minX - screenCenter.x) / fromCenter.x;

            float scaleY =
                (fromCenter.y > 0)
                ? (maxY - screenCenter.y) / fromCenter.y
                : (minY - screenCenter.y) / fromCenter.y;

            float scale =
                Mathf.Min(Mathf.Abs(scaleX), Mathf.Abs(scaleY));

            Vector2 edgePos =
                screenCenter + fromCenter * scale;

            screenPos.x = edgePos.x;
            screenPos.y = edgePos.y;
        }
        else
        {
            // Clamp
            screenPos.x =
                Mathf.Clamp(screenPos.x, minX, maxX);

            screenPos.y =
                Mathf.Clamp(screenPos.y, minY, maxY);
        }

        img.transform.position = screenPos;

        meter.text =
            ((int)Vector3.Distance(
                target.position,
                player.position
            )).ToString() + "m";
    }

    void UpdateTarget()
    {
        PickupSystem pickup =
            player.GetComponent<PickupSystem>();

        if (pickup.HasPackage())
        {
            GameObject delivery =
                SpawnManager.Instance.GetCurrentDeliveryPoint();

            if (delivery != null)
            {
                target = delivery.transform;
            }
        }
        else
        {
            GameObject package =
                SpawnManager.Instance.GetCurrentPackage();

            if (package != null)
            {
                target = package.transform;
            }
        }
    }
}