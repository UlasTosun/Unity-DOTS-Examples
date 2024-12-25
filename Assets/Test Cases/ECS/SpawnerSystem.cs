using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;



[CreateBefore(typeof(AnimatorSystem))]
[BurstCompile]
partial struct SpawnerSystem : ISystem {



    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ECB = new(Allocator.TempJob);
        EntityCommandBuffer.ParallelWriter ParallelECB = ECB.AsParallelWriter();

        foreach (var (spawnerSettings, ecsAnimator) in SystemAPI.Query<RefRO<SpawnerSettings>, RefRO<ECSAnimator>>()) {
            SpawnerJob spawnerJob = new() {
                SpawnerSettings = spawnerSettings.ValueRO,
                ECSAnimator = ecsAnimator.ValueRO,
                ParallelECB = ParallelECB
            };

            int objectCount = spawnerSettings.ValueRO.ObjectCount.x * spawnerSettings.ValueRO.ObjectCount.y;
            JobHandle jobHandle = spawnerJob.Schedule(objectCount, 32);
            jobHandle.Complete();
        }

        ECB.Playback(state.EntityManager);
        ECB.Dispose();

        state.Enabled = false; // disable the system to prevent multiple spawns
    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        
    }



}



[BurstCompile]
public struct SpawnerJob : IJobParallelFor {

    [ReadOnly] public SpawnerSettings SpawnerSettings;
    [ReadOnly] public ECSAnimator ECSAnimator;
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
            Scale = 1f
        };

        Entity entity = ParallelECB.Instantiate(i, SpawnerSettings.PrefabToSpawn);
        ParallelECB.SetComponent(i, entity, localTransform);
        ParallelECB.AddComponent(i, entity, ECSAnimator);
    }




}
