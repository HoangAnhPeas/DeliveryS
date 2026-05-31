using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Package")]
    public GameObject packagePrefab;

    [Header("Spawn Points")]
    public Transform[] packageSpawnPoints;

    [Header("Delivery Points")]
    public GameObject[] deliveryPoints;

    private GameObject currentPackage;
    private GameObject currentDeliveryPoint;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnNewDelivery();
    }

    public void SpawnNewDelivery()
    {
        SpawnPackage();
        SelectDeliveryPoint();
    }

    void SpawnPackage()
    {
        if (currentPackage != null)
        {
            Destroy(currentPackage);
        }

        Transform randomSpawn =
            packageSpawnPoints[
                Random.Range(0, packageSpawnPoints.Length)
            ];

        currentPackage = Instantiate(
            packagePrefab,
            randomSpawn.position,
            Quaternion.identity
        );
    }

    void SelectDeliveryPoint()
    {
        if (currentDeliveryPoint != null)
        {
            currentDeliveryPoint.SetActive(false);
        }

        currentDeliveryPoint =
            deliveryPoints[
                Random.Range(0, deliveryPoints.Length)
            ];

        currentDeliveryPoint.SetActive(true);
    }

    // ===== GETTERS =====

    public GameObject GetCurrentPackage()
    {
        return currentPackage;
    }

    public GameObject GetCurrentDeliveryPoint()
    {
        return currentDeliveryPoint;
    }
}