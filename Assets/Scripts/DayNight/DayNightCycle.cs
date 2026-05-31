using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Seconds for a full day-night cycle")]
    public float dayLength = 300f; // 5 minutes

    [Range(0f, 24f)]
    float timeOfDay = 12f;

    public DayNightAmbient ambient;

    void Update()
    {
        float speed = 360f / dayLength;

        transform.Rotate(Vector3.right, speed * Time.deltaTime, Space.Self);

        // sync time from rotation
        float angle = transform.eulerAngles.x;
        timeOfDay = (angle / 360f) * 24f;

        if (ambient != null)
            ambient.UpdateLighting(timeOfDay);
    }
}