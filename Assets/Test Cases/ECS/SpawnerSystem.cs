using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;



[CreateBefore(typeof(AnimatorSystem))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct SpawnerSystem : ISystem {



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SpawnerSettings>();
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        SpawnerSettings spawnerSettings = SystemAPI.GetSingleton<SpawnerSettings>();

        EntityCommandBuffer ECB = new(Allocator.TempJob); // Create an entity command buffer to make structural changes more efficiently.
        EntityCommandBuffer.ParallelWriter ParallelECB = ECB.AsParallelWriter(); // Create a parallel writer for ECB to be able to write in parallel jobs.

        SpawnerJob spawnerJob = new() {
            SpawnerSettings = spawnerSettings,
            ParallelECB = ParallelECB
        };

        int objectCount = spawnerSettings.ObjectCount.x * spawnerSettings.ObjectCount.y;
        JobHandle jobHandle = spawnerJob.Schedule(objectCount, 32);
        jobHandle.Complete();

        ECB.Playback(state.EntityManager); // Let ECB apply the changes to the EntityManager.
        ECB.Dispose();

        state.Enabled = false; // Disable the system to prevent multiple spawns.
    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        
    }



}



[BurstCompile]
public struct SpawnerJob : IJobParallelFor {

    [ReadOnly] public SpawnerSettings SpawnerSettings;
    public EntityCommandBuffer.ParallelWriter ParallelECB;



    [BurstCompile]
    public void Execute(int i) {

        float x = i % SpawnerSettings.ObjectCount.x;
        float z = math.floor(i / SpawnerSettings.ObjectCount.x);

        float xPosition = (x - (SpawnerSettings.ObjectCount.x - 1) / 2f) * SpawnerSettings.ObjectSpacing.x;
        float zPosition = (z - (SpawnerSettings.ObjectCount.y - 1) / 2f) * SpawnerSettings.ObjectSpacing.y;
        float3 position = new(xPosition, 0f, zPosition);

        LocalTransform localTransform = new(){
            Position = position,
            Rotation = quaternion.identity, // Do not forget to set rotation.
            Scale = 1f // Do not forget to set scale.
        };

        Entity entity = ParallelECB.Instantiate(i, SpawnerSettings.PrefabToSpawn);
        ParallelECB.SetComponent(i, entity, localTransform);
    }



}
