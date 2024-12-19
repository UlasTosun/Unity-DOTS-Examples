using Unity.Entities;
using Unity.Mathematics;



public struct SpawnerSettings : IComponentData {

    public Entity PrefabToSpawn;
    public int2 ObjectCount;
    public float2 ObjectSpacing;
    
}
