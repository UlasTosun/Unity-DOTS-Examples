using UnityEngine;
using Unity.Entities;



[RequireComponent(typeof(ECSAnimatorAuthoring))]
public class SpawnerSettingsAuthoring : MonoBehaviour {

    [Header("References")]
    [Tooltip("The prefab to spawn.")]
    public GameObject PrefabToSpawn;

    [Header("Settings")]
    [Tooltip("The number of objects to spawn on the X and Z axes.")]
    public Vector2Int ObjectCount = new(200, 200);
    [Tooltip("The distance between each object on the X and Z axes.")]
    public Vector2 ObjectSpacing = new(2f, 2f);

}



public class SpawnerSettingsBaker : Baker<SpawnerSettingsAuthoring> {



    public override void Bake(SpawnerSettingsAuthoring authoring) {
        
        Entity entity = GetEntity(TransformUsageFlags.None);

        SpawnerSettings settings = new() {
            PrefabToSpawn = GetEntity(authoring.PrefabToSpawn, TransformUsageFlags.Dynamic),
            ObjectCount = new(authoring.ObjectCount.x, authoring.ObjectCount.y),
            ObjectSpacing = new(authoring.ObjectSpacing.x, authoring.ObjectSpacing.y)            
        };

        AddComponent(entity, settings);

    }



}