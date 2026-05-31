using UnityEngine;

public class SimpleOcclusionCulling : MonoBehaviour
{
    public Camera targetCamera;
    public Transform player;

    [Header("Always Visible Around Player")]
    public float playerRadius = 0.5f;

    private Renderer[] renderers;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        renderers = GetComponentsInChildren<Renderer>(true);
    }

    void LateUpdate()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(targetCamera);

        foreach (Renderer rend in renderers)
        {
            if (rend == null)
                continue;

            bool nearPlayer =
                Vector3.Distance(
                    rend.bounds.center,
                    player.position
                ) <= playerRadius;

            bool inView =
                GeometryUtility.TestPlanesAABB(
                    planes,
                    rend.bounds
                );

            rend.enabled = nearPlayer || inView;
        }
    }
}