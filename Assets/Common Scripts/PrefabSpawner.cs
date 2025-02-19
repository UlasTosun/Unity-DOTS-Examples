using UnityEngine;



public class PrefabSpawner : MonoBehaviour {

    [Header("References")]
    [Tooltip("The prefab to spawn.")]
    public GameObject PrefabToSpawn;

    [Header("Settings")]
    [Tooltip("The number of objects to spawn on the X and Z axes.")]
    [Min(0)]
    public Vector2Int ObjectCount = new (200, 200);
    [Tooltip("The distance between each object on the X and Z axes.")]
    [Min(0)]
    public Vector2 ObjectSpacing = new (2f, 2f);

    [HideInInspector] public Transform[] SpawnedObjects;



    void Awake() {
        SpawnedObjects = new Transform[ObjectCount.x * ObjectCount.y];
        SpawnObjects();
    }



    public void SpawnObjects() {
        for (int x = 0; x < ObjectCount.x; x++) {
            for (int z = 0; z < ObjectCount.y; z++) {
                float xPosition = (x - (ObjectCount.x - 1) / 2f) * ObjectSpacing.x;
                float zPosition = (z - (ObjectCount.y - 1) / 2f) * ObjectSpacing.y;
                Vector3 position = new (xPosition, 0f, zPosition);
                Transform spawnedObject = Instantiate(PrefabToSpawn, position, Quaternion.identity, transform).transform;
                SpawnedObjects[x * ObjectCount.y + z] = spawnedObject;
            }
        }
    }



}
