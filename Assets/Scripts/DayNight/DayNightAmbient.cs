using UnityEngine;

public class DayNightAmbient : MonoBehaviour
{
    public Gradient ambientColor;
    public Gradient directionalColor;

    [Header("Fog Settings")]
    public Gradient fogColor;
    public AnimationCurve fogDensityCurve;

    private Light dirLight;

    void Awake()
    {
        dirLight = GetComponent<Light>();

        RenderSettings.fog = true; // Enable fog in the scene
    }

    public void UpdateLighting(float timeOfDay)
    {
        float t = (timeOfDay + 6f) / 24f;
        if (t < 0f) t += 1f;

        // LIGHTING
        RenderSettings.ambientLight = ambientColor.Evaluate(t);

        if (dirLight != null)
            dirLight.color = directionalColor.Evaluate(t);

        // FOG COLOR
        RenderSettings.fogColor = fogColor.Evaluate(t);

        // FOG DENSITY
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(t);

        // FOG DISTANCES
        RenderSettings.fogStartDistance = Mathf.Lerp(10f, 50f, t);
        RenderSettings.fogEndDistance = Mathf.Lerp(60f, 200f, t);

        // Optional: Add a slight boost to fog density at night for a more atmospheric effect
        RenderSettings.fogDensity *= 1.2f;
    }
}