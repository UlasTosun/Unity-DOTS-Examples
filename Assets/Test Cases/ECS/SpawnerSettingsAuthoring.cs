using Unity.Entities;



public class SpawnerSettingsAuthoring : PrefabSpawner {

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