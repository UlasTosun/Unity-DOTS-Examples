using Unity.Burst;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;



[CreateAfter(typeof(SpawnerSystem))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
partial struct AnimatorSystem : ISystem {



    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<ECSAnimator>();
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        ECSAnimator ecsAnimator = SystemAPI.GetSingleton<ECSAnimator>();
        EntityQuery query = SystemAPI.QueryBuilder().WithAll<LocalTransform>().Build();

        AnimatorJob animatorJob = new() {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            ECSAnimator = ecsAnimator
        };

        animatorJob.ScheduleParallel(query);
    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state) {
        
    }



}



[BurstCompile]
public partial struct AnimatorJob : IJobEntity {

    [ReadOnly] public double ElapsedTime;
    [ReadOnly] public ECSAnimator ECSAnimator;



    [BurstCompile]
    public void Execute(ref LocalTransform localTransform) {
        float x = localTransform.Position.x;
        float z = localTransform.Position.z;

        float xWave = math.sin(ECSAnimator.Frequency.x * x + ECSAnimator.PhaseMultiplier.x * (float) ElapsedTime);
        float zWave = math.sin(ECSAnimator.Frequency.y * z + ECSAnimator.PhaseMultiplier.y * (float) ElapsedTime);

        float y = ECSAnimator.Amplitude.x * xWave + ECSAnimator.Amplitude.y * zWave;
        localTransform.Position = new float3(x, y, z);
    }



}
